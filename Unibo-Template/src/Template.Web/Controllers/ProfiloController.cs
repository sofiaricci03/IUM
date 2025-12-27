using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Template.Services;

namespace Template.Web.Controllers
{
    [Authorize]
    public partial class ProfiloController : Controller
    {
        private readonly TemplateDbContext _ctx;

        public ProfiloController(TemplateDbContext ctx)
        {
            _ctx = ctx;
        }

        // Serve l'immagine profilo
        [HttpGet]
        [Route("Profilo/Immagine/{dipendenteId?}")]
        public virtual async Task<IActionResult> Immagine(int? dipendenteId = null)
        {
            Template.Entities.Dipendente dipendente;
            
            if (dipendenteId == null)
            {
                // Usa l'utente corrente
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return NotFound();
                
                dipendente = await _ctx.Dipendenti
                    .FirstOrDefaultAsync(d => d.UserId.ToString() == userId);
            }
            else
            {
                dipendente = await _ctx.Dipendenti.FindAsync(dipendenteId.Value);
            }

            if (dipendente?.ProfileImage != null && dipendente.ProfileImage.Length > 0)
            {
                return File(dipendente.ProfileImage, dipendente.ProfileImageContentType ?? "image/jpeg");
            }
            
            // Ritorna immagine default SVG
            var svg = GenerateDefaultAvatar(dipendente?.Nome ?? "U");
            return File(System.Text.Encoding.UTF8.GetBytes(svg), "image/svg+xml");
        }

        // Upload nuova foto profilo
        [HttpPost]
        public virtual async Task<IActionResult> UploadFoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "Nessun file selezionato" });

            // Validazione tipo file
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                return BadRequest(new { error = "Formato non supportato. Usa JPG, PNG o GIF." });

            // Validazione dimensione (max 5MB)
            if (file.Length > 5 * 1024 * 1024)
                return BadRequest(new { error = "File troppo grande. Max 5MB." });

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var dipendente = await _ctx.Dipendenti
                .FirstOrDefaultAsync(d => d.UserId.ToString() == userId);
            
            if (dipendente == null)
                return NotFound(new { error = "Dipendente non trovato" });

            // Leggi il file in byte array
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                dipendente.ProfileImage = memoryStream.ToArray();
                dipendente.ProfileImageContentType = file.ContentType;
            }

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Foto profilo aggiornata!" });
        }

        // Rimuovi foto profilo
        [HttpPost]
        public virtual async Task<IActionResult> RimuoviFoto()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var dipendente = await _ctx.Dipendenti
                .FirstOrDefaultAsync(d => d.UserId.ToString() == userId);
            
            if (dipendente == null)
                return NotFound();

            dipendente.ProfileImage = null;
            dipendente.ProfileImageContentType = null;

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Foto profilo rimossa" });
        }

        // Genera avatar SVG di default
        private string GenerateDefaultAvatar(string initials)
        {
            var initial = initials.Substring(0, Math.Min(1, initials.Length)).ToUpper();
            var colors = new[] { "#3788d8", "#28a745", "#fd7e14", "#6f42c1", "#e83e8c", "#20c997" };
            var color = colors[initial[0] % colors.Length];

            return $@"
                <svg width=""100"" height=""100"" xmlns=""http://www.w3.org/2000/svg"">
                    <circle cx=""50"" cy=""50"" r=""50"" fill=""{color}""/>
                    <text x=""50"" y=""50"" text-anchor=""middle"" dominant-baseline=""middle"" 
                          font-family=""Arial, sans-serif"" font-size=""45"" fill=""white"" font-weight=""bold"">
                        {initial}
                    </text>
                </svg>";
        }
    }
}