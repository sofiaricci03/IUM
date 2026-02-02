using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
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
    public partial class CalendarioApiController : ControllerBase
    {
        private readonly TemplateDbContext _ctx;

        public CalendarioApiController(TemplateDbContext ctx)
        {
            _ctx = ctx;
        }

        private Template.Entities.Dipendente GetDipendente()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
        }

        [HttpGet]
        public virtual IActionResult GetAttivita()
        {
            var dip = GetDipendente();
            if (dip == null)
                return BadRequest(new { error = "Dipendente non trovato. Fai logout/login." });

            var attivita = _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == dip.Id)
                .ToList()
                .Select(a =>
                {
                    var progetto = _ctx.Progetti.FirstOrDefault(p => p.Id == a.ProgettoId);
                    return new AttivitaDTO
                    {
                        id = a.Id,
                        progetto = progetto?.Nome ?? "",
                        progettoId = a.ProgettoId,
                        cliente = progetto?.Cliente ?? "",
                        attivita = a.Attivita ?? "",
                        descrizione = a.Descrizione ?? "",
                        start = a.Giorno.ToString("yyyy-MM-dd") + "T" + a.OraInizio.ToString(@"hh\:mm"),
                        end = a.Giorno.ToString("yyyy-MM-dd") + "T" + a.OraFine.ToString(@"hh\:mm"),
                        trasferta = a.Trasferta,
                        spesaTrasporto = a.SpesaTrasporto,
                        spesaCibo = a.SpesaCibo,
                        spesaAlloggio = a.SpesaAlloggio
                    };
                })
                .ToList();

            return Ok(attivita);
        }

        [HttpGet]
        public virtual IActionResult GetProgetti()
        {
            try
            {
                var dip = GetDipendente();
                if (dip == null)
                    return BadRequest(new { error = "Dipendente non trovato" });

                var progetti = _ctx.Progetti
                    .Where(p => !p.Completato)
                    .Select(p => new
                    {
                        id = p.Id,
                        nome = p.Nome,
                        cliente = p.Cliente
                    })
                    .ToList();

                return Ok(progetti);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public virtual IActionResult AddAttivita([FromBody] AttivitaDTO dto)
        {
            var dip = GetDipendente();
            if (dip == null)
                return BadRequest(new { error = "Dipendente non trovato. Fai logout/login." });

            if (dto.progettoId == 0)
                return BadRequest(new { error = "Seleziona un progetto" });

            var progetto = _ctx.Progetti.FirstOrDefault(p => p.Id == dto.progettoId);
            if (progetto == null)
                return BadRequest(new { error = "Progetto non trovato" });

            if (progetto.Completato)
                return BadRequest(new { error = "Il progetto è completato" });

            if (!TimeSpan.TryParse(dto.oraInizio, out var oraInizio) || 
                !TimeSpan.TryParse(dto.oraFine, out var oraFine))
                return BadRequest(new { error = "Orari non validi" });

            if (oraFine <= oraInizio)
                return BadRequest(new { error = "L'ora di fine deve essere dopo l'ora di inizio" });

            if (!DateTime.TryParse(dto.giorno, out var giorno))
                return BadRequest(new { error = "Data non valida" });

            var durata = (oraFine - oraInizio).TotalHours;

            var sovrapposizione = _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == dip.Id && a.Giorno.Date == giorno.Date)
                .AsEnumerable()
                .Any(a =>
                    (oraInizio >= a.OraInizio && oraInizio < a.OraFine) ||
                    (oraFine > a.OraInizio && oraFine <= a.OraFine) ||
                    (oraInizio <= a.OraInizio && oraFine >= a.OraFine));

            if (sovrapposizione)
                return BadRequest(new { error = "Sovrapposizione con un'altra attività" });

            var oreGiorno = _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == dip.Id && a.Giorno.Date == giorno.Date)
                .AsEnumerable()
                .Sum(a => (a.OraFine - a.OraInizio).TotalHours);

            if (oreGiorno + durata > 12)
                return BadRequest(new { error = $"Superato limite 12h/giorno. Già inserite: {oreGiorno:F1}h" });

            var attivita = new AttivitaLavorativa
            {
                DipendenteId = dip.Id,
                ProgettoId = progetto.Id,
                Giorno = giorno,
                OraInizio = oraInizio,
                OraFine = oraFine,
                Attivita = dto.attivita,
                Descrizione = dto.descrizione,
                Trasferta = dto.trasferta,
                SpesaTrasporto = dto.spesaTrasporto,
                SpesaCibo = dto.spesaCibo,
                SpesaAlloggio = dto.spesaAlloggio
            };

            _ctx.AttivitaLavorative.Add(attivita);
            _ctx.SaveChanges();

            return Ok(new { message = "Attività salvata!", id = attivita.Id });
        }

        [HttpPost]
        public virtual IActionResult UpdateAttivita([FromBody] AttivitaDTO dto)
        {
            var dip = GetDipendente();
            if (dip == null)
                return BadRequest(new { error = "Dipendente non trovato" });

            var attivita = _ctx.AttivitaLavorative
                .FirstOrDefault(a => a.Id == dto.id && a.DipendenteId == dip.Id);

            if (attivita == null)
                return NotFound(new { error = "Attività non trovata" });

            if (!TimeSpan.TryParse(dto.oraInizio, out var oraInizio) || 
                !TimeSpan.TryParse(dto.oraFine, out var oraFine))
                return BadRequest(new { error = "Orari non validi" });

            if (oraFine <= oraInizio)
                return BadRequest(new { error = "L'ora di fine deve essere dopo l'ora di inizio" });

            if (!DateTime.TryParse(dto.giorno, out var giorno))
                return BadRequest(new { error = "Data non valida" });

            var sovrapposizione = _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == dip.Id && a.Giorno.Date == giorno.Date && a.Id != dto.id)
                .AsEnumerable()
                .Any(a =>
                    (oraInizio >= a.OraInizio && oraInizio < a.OraFine) ||
                    (oraFine > a.OraInizio && oraFine <= a.OraFine) ||
                    (oraInizio <= a.OraInizio && oraFine >= a.OraFine));

            if (sovrapposizione)
                return BadRequest(new { error = "Sovrapposizione con un'altra attività" });

            var durata = (oraFine - oraInizio).TotalHours;
            var oreGiorno = _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == dip.Id && a.Giorno.Date == giorno.Date && a.Id != dto.id)
                .AsEnumerable()
                .Sum(a => (a.OraFine - a.OraInizio).TotalHours);

            if (oreGiorno + durata > 12)
                return BadRequest(new { error = $"Superato limite 12h/giorno. Già inserite: {oreGiorno:F1}h" });

            attivita.ProgettoId = dto.progettoId;
            attivita.Giorno = giorno;
            attivita.OraInizio = oraInizio;
            attivita.OraFine = oraFine;
            attivita.Attivita = dto.attivita;
            attivita.Descrizione = dto.descrizione;
            attivita.Trasferta = dto.trasferta;
            attivita.SpesaTrasporto = dto.spesaTrasporto;
            attivita.SpesaCibo = dto.spesaCibo;
            attivita.SpesaAlloggio = dto.spesaAlloggio;

            _ctx.SaveChanges();

            return Ok(new { message = "Attività aggiornata!" });
        }

        [HttpPost]
        public virtual IActionResult DeleteAttivita([FromBody] int id)
        {
            var dip = GetDipendente();
            if (dip == null)
                return Unauthorized();

            var attivita = _ctx.AttivitaLavorative
                .FirstOrDefault(a => a.Id == id && a.DipendenteId == dip.Id);

            if (attivita == null)
                return NotFound(new { error = "Attività non trovata" });

            _ctx.AttivitaLavorative.Remove(attivita);
            _ctx.SaveChanges();

            return Ok(new { message = "Attività eliminata!" });
        }
    }
}