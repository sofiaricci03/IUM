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

namespace Template.Web.Areas.Dipendente.Controllers
{
    [Area("Dipendente")]
    [Authorize(Roles = nameof(UserRole.Dipendente))]
    [ApiController]
    [Route("Dipendente/[controller]/[action]")]
    public partial class CongedoApiController : ControllerBase
    {
        private readonly TemplateDbContext _ctx;

        public CongedoApiController(TemplateDbContext ctx)
        {
            _ctx = ctx;
        }

        // =========================
        // GET – TUTTE LE FERIE (tutti i dipendenti)
        // =========================
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
                end = r.Tipo == "Ferie" || r.Tipo == "Malattia" ? r.DataFine.AddDays(1) : (DateTime?)null,
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

        // =========================
        // GET – TUTTI I DIPENDENTI
        // =========================
        [HttpGet]
        public virtual async Task<IActionResult> GetDipendenti()
        {
            var dipendenti = await _ctx.Dipendenti
                .OrderBy(d => d.Cognome)
                .ThenBy(d => d.Nome)
                .Select(d => new
                {
                    id = d.Id,
                    nomeCompleto = d.Nome + " " + d.Cognome
                })
                .ToListAsync();
            return Ok(dipendenti);
        }

        // =========================
        // GET – INFO DIPENDENTE CORRENTE
        // =========================
        [HttpGet]
        public virtual IActionResult GetDipendenteCorrente()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dip = _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
            if (dip == null) return Unauthorized();

            return Ok(new
            {
                id = dip.Id,
                nomeCompleto = dip.Nome + " " + dip.Cognome
            });
        }

        // =========================
        // GET – Eventi calendario (solo mie richieste)
        // =========================
        [HttpGet]
        public virtual IActionResult GetRichieste()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dip = _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
            if (dip == null) return Unauthorized();

            var richieste = _ctx.RichiestaFerie
                .Where(r => r.DipendenteId == dip.Id)
                .Select(r => new
                {
                    id = r.Id,
                    title = r.Tipo,
                    start = r.DataInizio,
                    end = r.Tipo == "Ferie"
                        ? r.DataFine.AddDays(1)
                        : (DateTime?)null,
                    allDay = true,

                    color = r.Tipo == "Ferie" ? "#ff4d4d" : "#ffa500",

                    extendedProps = new
                    {
                        motivo = r.Motivo,
                        tipo = r.Tipo,
                        stato = r.Stato
                    }
                })
                .ToList();

            return Ok(richieste);
        }

        // =========================
        // POST – nuova richiesta
        // =========================
        [HttpPost]
        public virtual IActionResult Richiedi([FromBody] RichiestaFerieDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // DEBUG
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new { error = "NameIdentifier claim non trovato. Fai logout/login." });
            
            var dip = _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
            
            // DEBUG
            if (dip == null)
                return BadRequest(new { error = $"Dipendente non trovato per UserId: {userId}" });

            // DEBUG
            if (string.IsNullOrEmpty(dto.dal))
                return BadRequest(new { error = "Campo 'dal' vuoto" });

            if (!DateTime.TryParse(dto.dal, out var inizio))
                return BadRequest(new { error = $"Data inizio non valida. Ricevuto: {dto.dal}" });

            DateTime fine;
            if (dto.tipo == "Permesso")
            {
                fine = inizio;
            }
            else
            {
                if (string.IsNullOrEmpty(dto.al))
                    return BadRequest(new { error = "Campo 'al' vuoto per tipo Ferie" });
                    
                if (!DateTime.TryParse(dto.al, out fine))
                    return BadRequest(new { error = $"Data fine non valida. Ricevuto: {dto.al}" });
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
            _ctx.SaveChanges();

            return Ok(new { message = "Richiesta inviata con successo!" });
        }

        // =========================
        // POST – modifica richiesta
        // =========================
        [HttpPost]
        public virtual IActionResult Update([FromBody] RichiestaFerieDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dip = _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
            if (dip == null) return Unauthorized();

            var richiesta = _ctx.RichiestaFerie
                .FirstOrDefault(r => r.Id == dto.id && r.DipendenteId == dip.Id);

            if (richiesta == null)
                return NotFound();

            if (richiesta.Stato != FerieStato.InAttesa)
                return BadRequest();

            if (!DateTime.TryParse(dto.dal, out var inizio))
                return BadRequest();

            DateTime fine;
            if (richiesta.Tipo == "Permesso")
                fine = inizio;
            else if (!DateTime.TryParse(dto.al, out fine))
                return BadRequest();

            richiesta.DataInizio = inizio;
            richiesta.DataFine = fine;
            richiesta.Motivo = dto.motivo;

            _ctx.SaveChanges();
            return Ok();
        }

        // =========================
        // POST – elimina richiesta
        // =========================
        [HttpPost]
        public virtual IActionResult Delete([FromBody] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dip = _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
            if (dip == null) return Unauthorized();

            var richiesta = _ctx.RichiestaFerie
                .FirstOrDefault(r => r.Id == id && r.DipendenteId == dip.Id);

            if (richiesta == null)
                return NotFound();

            _ctx.RichiestaFerie.Remove(richiesta);
            _ctx.SaveChanges();

            return Ok();
        }
    }
}
