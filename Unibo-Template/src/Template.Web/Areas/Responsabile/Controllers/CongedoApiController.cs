using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Template.Entities;
using Template.Services;
using Template.Services.Shared;
using Template.Web.Areas.Dipendente.Models;

namespace Template.Web.Areas.Responsabile.Controllers
{
    [Area("Responsabile")]
    [Authorize(Roles = nameof(UserRole.Responsabile))]
    [ApiController]
    [Route("Responsabile/[controller]/[action]")]
    public partial class CongedoApiController : ControllerBase
    {
        private readonly TemplateDbContext _ctx;

        public CongedoApiController(TemplateDbContext ctx)
        {
            _ctx = ctx;
        }

        // ========================================
        // GET - TUTTE LE FERIE (responsabile vede tutti)
        // ========================================
        [HttpGet]
        public virtual async Task<IActionResult> GetFerieTutti()
        {
            var richieste = await _ctx.RichiestaFerie
                .Include(r => r.Dipendente)
                .Where(r => r.Stato != FerieStato.Rifiutato)
                .ToListAsync();
            var risultato = richieste.Select(r => new
            {
                id = r.Id,
                title = r.Dipendente.Nome + " " + r.Dipendente.Cognome + " - " + r.Tipo,
                start = r.DataInizio,
                end = r.Tipo == "Ferie" ? r.DataFine.AddDays(1) : (DateTime?)null,
                allDay = true,
                color = r.Stato == FerieStato.InAttesa ? "#ffc107" : "#28a745",
                extendedProps = new
                {
                    dipendenteId = r.DipendenteId,
                    dipendente = r.Dipendente.Nome + " " + r.Dipendente.Cognome,
                    motivo = r.Motivo,
                    tipo = r.Tipo,
                    stato = r.Stato.ToString()
                }
            }).ToList();

            return Ok(risultato);
        }

        // ========================================
        // GET - LE MIE FERIE (responsabile inserisce le sue)
        // ========================================
        [HttpGet]
        public virtual async Task<IActionResult> GetMieFerie()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dip = await _ctx.Dipendenti.FirstOrDefaultAsync(d => d.UserId.ToString() == userId);

            if (dip == null)
                return BadRequest(new { error = "Dipendente non trovato" });

            var richieste = await _ctx.RichiestaFerie
                .Where(r => r.DipendenteId == dip.Id && r.Stato != FerieStato.Rifiutato)
                .ToListAsync();

            var risultato = richieste.Select(r => new
            {
                id = r.Id,
                title = r.Tipo,
                start = r.DataInizio,
                end = r.Tipo == "Ferie" ? r.DataFine.AddDays(1) : (DateTime?)null,
                allDay = true,
                color = r.Stato == FerieStato.InAttesa ? "#ffc107" : "#28a745",
                extendedProps = new
                {
                    motivo = r.Motivo,
                    tipo = r.Tipo,
                    stato = r.Stato.ToString()
                }
            }).ToList();

            return Ok(risultato);
        }

        // ========================================
        // GET - RICHIESTE DA APPROVARE
        // ========================================
        [HttpGet]
        public virtual async Task<IActionResult> GetRichiesteDaApprovare()
        {
            var richieste = await _ctx.RichiestaFerie
                .Include(r => r.Dipendente)
                .Where(r => r.Stato == FerieStato.InAttesa)
                .OrderByDescending(r => r.DataRichiesta)
                .ToListAsync();

            var risultato = richieste.Select(r => new
            {
                id = r.Id,
                dipendente = r.Dipendente.Nome + " " + r.Dipendente.Cognome,
                tipo = r.Tipo,
                dataInizio = r.DataInizio.ToString("dd/MM/yyyy"),
                dataFine = r.DataFine.ToString("dd/MM/yyyy"),
                motivo = r.Motivo,
                dataRichiesta = r.DataRichiesta.ToString("dd/MM/yyyy HH:mm"),
                giorni = (r.DataFine - r.DataInizio).Days + 1
            }).ToList();

            return Ok(risultato);
        }
        // ========================================
        // POST - RICHIEDI FERIE (responsabile inserisce le sue)
        // ========================================
        [HttpPost]
        public virtual async Task<IActionResult> Richiedi([FromBody] RichiestaFerieDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dip = await _ctx.Dipendenti.FirstOrDefaultAsync(d => d.UserId.ToString() == userId);
            
            if (dip == null)
                return BadRequest(new { error = "Dipendente non trovato. Fai logout/login." });

            if (!DateTime.TryParse(dto.dal, out var inizio))
                return BadRequest(new { error = "Data inizio non valida" });

            DateTime fine;
            if (dto.tipo == "Permesso")
            {
                fine = inizio;
            }
            else
            {
                if (!DateTime.TryParse(dto.al, out fine))
                    return BadRequest(new { error = "Data fine non valida" });
            }

            var richiesta = new RichiestaFerie
            {
                DipendenteId = dip.Id,
                DataInizio = inizio,
                DataFine = fine,
                Motivo = dto.motivo,
                Tipo = dto.tipo,
                Stato = FerieStato.InAttesa,
                DataRichiesta = DateTime.Now
            };

            _ctx.RichiestaFerie.Add(richiesta);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Richiesta inviata!" });
        }

        // ========================================
        // POST - APPROVA RICHIESTA
        // ========================================
        [HttpPost]
        public virtual async Task<IActionResult> Approva([FromBody] int id)
        {
            var richiesta = await _ctx.RichiestaFerie.FindAsync(id);
            
            if (richiesta == null)
                return NotFound(new { error = "Richiesta non trovata" });

            if (richiesta.Stato != FerieStato.InAttesa)
                return BadRequest(new { error = "La richiesta non è in attesa" });

            richiesta.Stato = FerieStato.Approvato;
            richiesta.DataRisposta = DateTime.Now;

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Richiesta approvata!" });
        }

        // ========================================
        // POST - RESPINGI RICHIESTA
        // ========================================
        [HttpPost]
        public virtual async Task<IActionResult> Respingi([FromBody] RespingiRichiestaRequest request)
        {
            var richiesta = await _ctx.RichiestaFerie.FindAsync(request.Id);
            
            if (richiesta == null)
                return NotFound(new { error = "Richiesta non trovata" });

            if (richiesta.Stato != FerieStato.InAttesa)
                return BadRequest(new { error = "La richiesta non è in attesa" });

            richiesta.Stato = FerieStato.Rifiutato;
            richiesta.DataRisposta = DateTime.Now;
            richiesta.NoteResponsabile = request.Note;

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Richiesta respinta" });
        }

        // ========================================
        // POST - ELIMINA RICHIESTA
        // ========================================
        [HttpPost]
        public virtual async Task<IActionResult> Elimina([FromBody] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dip = await _ctx.Dipendenti.FirstOrDefaultAsync(d => d.UserId.ToString() == userId);
            
            if (dip == null)
                return Unauthorized();

            var richiesta = await _ctx.RichiestaFerie
                .FirstOrDefaultAsync(r => r.Id == id && r.DipendenteId == dip.Id);

            if (richiesta == null)
                return NotFound();

            if (richiesta.Stato == FerieStato.Approvato)
                return BadRequest(new { error = "Non puoi eliminare una richiesta già approvata" });

            _ctx.RichiestaFerie.Remove(richiesta);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Richiesta eliminata" });
        }
    }

    public class RespingiRichiestaRequest
    {
        public int Id { get; set; }
        public string Note { get; set; }
    }
}