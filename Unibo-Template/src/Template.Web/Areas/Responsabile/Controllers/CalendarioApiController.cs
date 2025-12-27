using Template.Web.Areas.Dipendente.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Template.Services;
using Template.Entities;
using System.Linq;
using System;

namespace Template.Web.Areas.Responsabile.Controllers
{
    [Area("Responsabile")]
    [Authorize(Roles = nameof(Template.Services.Shared.UserRole.Responsabile))]
    [ApiController]
    [Route("Responsabile/[controller]/[action]")]
    public partial class CalendarioApiController : Controller
    {
        private readonly TemplateDbContext _ctx;

        public CalendarioApiController(TemplateDbContext ctx)
        {
            _ctx = ctx;
        }

        // ==========================
        // GET attività responsabile
        // ==========================
        [HttpGet]
        public virtual IActionResult GetAttivita()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var dip = _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
            if (dip == null) 
            {
                // Il responsabile non ha un record Dipendente, ritorna vuoto
                return Ok(new object[] { });
            }

            var lista = _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == dip.Id)
                .Select(a => new
                {
                    id = a.Id,
                    title = $"{a.ProgettoNome} - {a.Attivita} ({a.Cliente})",
                    start = a.Giorno.Date + a.OraInizio,
                    end = a.Giorno.Date + a.OraFine,
                    progetto = a.ProgettoNome,
                    cliente = a.Cliente,
                    attivita = a.Attivita,
                    descrizione = a.Descrizione,
                    trasferta = a.Trasferta,
                    spesaTrasporto = a.SpesaTrasporto,
                    spesaCibo = a.SpesaCibo,
                    spesaAlloggio = a.SpesaAlloggio
                })
                .ToList();

            return Ok(lista);
        }

        // ==========================
        // POST aggiungi nuova attività
        // ==========================
        [HttpPost]
        public virtual async Task<IActionResult> AddAttivita([FromBody] AttivitaDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var dip = _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
            if (dip == null) 
            {
                // Crea un record Dipendente per il responsabile al volo
                var user = _ctx.Users.FirstOrDefault(u => u.Id.ToString() == userId);
                if (user == null) return Unauthorized();
                
                dip = new Template.Entities.Dipendente
                {
                    UserId = user.Id,
                    Nome = user.FirstName,
                    Cognome = user.LastName,
                    OreSettimanaliContratto = 40,
                    FerieResidue = 20,
                    PermessiResidui = 10,
                    GiorniMalattiaDisponibili = 10
                };
                
                _ctx.Dipendenti.Add(dip);
                await _ctx.SaveChangesAsync();
            }

            if (!DateTime.TryParse(dto.giorno, out var giorno))
                return BadRequest("Data non valida");
            if (!TimeSpan.TryParse(dto.oraInizio, out var oraInizio))
                return BadRequest("Ora inizio non valida");
            if (!TimeSpan.TryParse(dto.oraFine, out var oraFine))
                return BadRequest("Ora fine non valida");

            var entity = new AttivitaLavorativa
            {
                DipendenteId = dip.Id,

                Giorno = giorno,
                OraInizio = oraInizio,
                OraFine = oraFine,

                ProgettoNome = dto.progetto,
                Cliente = dto.cliente,

                Attivita = dto.attivita,
                Descrizione = dto.descrizione,

                Trasferta = dto.trasferta,
                SpesaTrasporto = dto.spesaTrasporto,
                SpesaCibo = dto.spesaCibo,
                SpesaAlloggio = dto.spesaAlloggio
            };

            _ctx.AttivitaLavorative.Add(entity);
            await _ctx.SaveChangesAsync();

            return Ok();
        }

        // ==========================
        // POST modifica attività esistente
        // ==========================
        [HttpPost]
        public virtual async Task<IActionResult> UpdateAttivita([FromBody] AttivitaDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var dip = _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
            if (dip == null) return Unauthorized();

            var entity = _ctx.AttivitaLavorative
                .FirstOrDefault(a => a.Id == dto.id && a.DipendenteId == dip.Id);

            if (entity == null)
                return NotFound("Attività non trovata.");

            entity.Giorno = DateTime.Parse(dto.giorno);
            entity.OraInizio = TimeSpan.Parse(dto.oraInizio);
            entity.OraFine = TimeSpan.Parse(dto.oraFine);

            entity.ProgettoNome = dto.progetto;
            entity.Cliente = dto.cliente;

            entity.Attivita = dto.attivita;
            entity.Descrizione = dto.descrizione;

            entity.Trasferta = dto.trasferta;
            entity.SpesaTrasporto = dto.spesaTrasporto;
            entity.SpesaCibo = dto.spesaCibo;
            entity.SpesaAlloggio = dto.spesaAlloggio;

            await _ctx.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public virtual async Task<IActionResult> DeleteAttivita([FromBody] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var dip = _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
            if (dip == null) return Unauthorized();

            var entity = _ctx.AttivitaLavorative
                .FirstOrDefault(a => a.Id == id && a.DipendenteId == dip.Id);

            if (entity == null)
                return NotFound();

            _ctx.AttivitaLavorative.Remove(entity);
            await _ctx.SaveChangesAsync();

            return Ok();
        }
    }
}