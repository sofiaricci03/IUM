using Template.Web.Areas.Dipendente.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Template.Services;
using Template.Entities;
using System.Linq;
using System;
using Template.Web.Areas;
using Microsoft.EntityFrameworkCore;

namespace Template.Web.Areas.Dipendente.Controllers
{
    [Area("Dipendente")]
    [Authorize(Roles = nameof(Template.Services.Shared.UserRole.Dipendente))]
    [ApiController]
    [Route("Dipendente/[controller]/[action]")]
    public partial class CalendarioApiController : BaseAreaController
    {
        private readonly TemplateDbContext _ctx;

        public CalendarioApiController(TemplateDbContext ctx)
        {
            _ctx = ctx;
        }

        // ==========================
        // GET attività dipendente
        // ==========================
        [HttpGet]
        public virtual IActionResult GetAttivita()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var dip = _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
            if (dip == null) return Unauthorized();

            var lista = _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == dip.Id)
                .Select(a => new
                {
                    id = a.Id,
                    title = $"{a.ProgettoNome} - {a.Attivita} ({a.Cliente})",
                    start = a.Giorno.Date + a.OraInizio,
                    end = a.Giorno.Date + a.OraFine,
                    progettoId = a.ProgettoId,
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
        // POST aggiungi nuova attività CON VALIDAZIONI
        // ==========================
        [HttpPost]
        public virtual async Task<IActionResult> AddAttivita([FromBody] AttivitaDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var dip = _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
            if (dip == null) return Unauthorized();

            // VALIDAZIONI BASE
            if (!DateTime.TryParse(dto.giorno, out var giorno))
                return BadRequest(new { error = "Data non valida" });
            if (!TimeSpan.TryParse(dto.oraInizio, out var oraInizio))
                return BadRequest(new { error = "Ora inizio non valida" });
            if (!TimeSpan.TryParse(dto.oraFine, out var oraFine))
                return BadRequest(new { error = "Ora fine non valida" });

            // Ora fine > ora inizio
            if (oraFine <= oraInizio)
                return BadRequest(new { error = "L'orario di fine deve essere successivo all'orario di inizio" });

            //  Max 12 ore singola attività
            var durataOre = (oraFine - oraInizio).TotalHours;
            if (durataOre > 12)
                return BadRequest(new { error = "Non puoi inserire più di 12 ore in una singola attività" });

            //  Verifica progetto assegnato
            var progettoId = dto.progettoId;
            var assegnato = _ctx.AssegnazioniDipendentiProgetti
                .Any(a => a.DipendenteId == dip.Id && a.ProgettoId == progettoId && a.Attivo);
            
            if (!assegnato)
                return BadRequest(new { error = "Non sei assegnato a questo progetto" });

            //  Progetto non completato
            var progetto = _ctx.Progetti.Find(progettoId);
            if (progetto == null)
                return BadRequest(new { error = "Progetto non trovato" });
            
            if (progetto.Completato)
                return BadRequest(new { error = "Non puoi rendicontare su un progetto completato" });

            //  Data dentro periodo progetto
            if (giorno.Date < progetto.DataInizio.Date)
                return BadRequest(new { error = $"Non puoi rendicontare prima della data inizio progetto ({progetto.DataInizio:dd/MM/yyyy})" });

            // Nessuna sovrapposizione oraria
            var sovrapposizioni = _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == dip.Id && a.Giorno.Date == giorno.Date)
                .AsEnumerable()
                .Where(a => 
                    (oraInizio >= a.OraInizio && oraInizio < a.OraFine) ||
                    (oraFine > a.OraInizio && oraFine <= a.OraFine) ||
                    (oraInizio <= a.OraInizio && oraFine >= a.OraFine))
                .ToList();

            if (sovrapposizioni.Any())
                return BadRequest(new { error = "Esiste già un'attività in questo orario" });

            // Totale ore giornaliere <= 12
            var oreTotaliGiorno = _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == dip.Id && a.Giorno.Date == giorno.Date)
                .AsEnumerable()
                .Sum(a => (a.OraFine - a.OraInizio).TotalHours);

            if (oreTotaliGiorno + durataOre > 12)
                return BadRequest(new { error = $"Superato limite 12 ore/giorno. Hai già {oreTotaliGiorno:F1}h inserite" });

            // Verifica se rendicontazione del mese è già stata inviata
            var rendicontazione = _ctx.RendicontazioniMensili
                .FirstOrDefault(r => r.DipendenteId == dip.Id && 
                                    r.Anno == giorno.Year && 
                                    r.Mese == giorno.Month);

            if (rendicontazione != null && rendicontazione.Stato != RendicontazioneStato.Bozza && rendicontazione.Stato != RendicontazioneStato.Respinta)
                return BadRequest(new { error = "La rendicontazione di questo mese è già stata inviata e non può essere modificata" });

            // TUTTO OK - Salva
            var entity = new AttivitaLavorativa
            {
                DipendenteId = dip.Id,
                ProgettoId = progettoId,
                Giorno = giorno,
                OraInizio = oraInizio,
                OraFine = oraFine,
                ProgettoNome = dto.progetto,
                Cliente = dto.cliente,
                Attivita = dto.attivita,
                Descrizione = dto.descrizione ?? "",
                Trasferta = dto.trasferta,
                SpesaTrasporto = dto.spesaTrasporto,
                SpesaCibo = dto.spesaCibo,
                SpesaAlloggio = dto.spesaAlloggio
            };

            _ctx.AttivitaLavorative.Add(entity);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Attività salvata con successo!" });
        }

        // ==========================
        // POST modifica attività esistente CON VALIDAZIONI
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
                return NotFound(new { error = "Attività non trovata" });

            // VALIDAZIONI (stesse di AddAttivita)
            if (!DateTime.TryParse(dto.giorno, out var giorno))
                return BadRequest(new { error = "Data non valida" });
            if (!TimeSpan.TryParse(dto.oraInizio, out var oraInizio))
                return BadRequest(new { error = "Ora inizio non valida" });
            if (!TimeSpan.TryParse(dto.oraFine, out var oraFine))
                return BadRequest(new { error = "Ora fine non valida" });

            if (oraFine <= oraInizio)
                return BadRequest(new { error = "L'orario di fine deve essere successivo all'orario di inizio" });

            // Verifica sovrapposizioni (escludendo l'attività corrente)
            var sovrapposizioni = _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == dip.Id && a.Giorno.Date == giorno.Date && a.Id != dto.id)
                .AsEnumerable()
                .Where(a => 
                    (oraInizio >= a.OraInizio && oraInizio < a.OraFine) ||
                    (oraFine > a.OraInizio && oraFine <= a.OraFine) ||
                    (oraInizio <= a.OraInizio && oraFine >= a.OraFine))
                .ToList();

            if (sovrapposizioni.Any())
                return BadRequest(new { error = "Esiste già un'attività in questo orario" });

            // Verifica rendicontazione non inviata
            var rendicontazione = _ctx.RendicontazioniMensili
                .FirstOrDefault(r => r.DipendenteId == dip.Id && 
                                    r.Anno == giorno.Year && 
                                    r.Mese == giorno.Month);

            if (rendicontazione != null && rendicontazione.Stato != RendicontazioneStato.Bozza && rendicontazione.Stato != RendicontazioneStato.Respinta)
                return BadRequest(new { error = "La rendicontazione di questo mese è già stata inviata e non può essere modificata" });

            // Aggiorna
            entity.Giorno = giorno;
            entity.OraInizio = oraInizio;
            entity.OraFine = oraFine;
            entity.ProgettoId = dto.progettoId;
            entity.ProgettoNome = dto.progetto;
            entity.Cliente = dto.cliente;
            entity.Attivita = dto.attivita;
            entity.Descrizione = dto.descrizione ?? "";
            entity.Trasferta = dto.trasferta;
            entity.SpesaTrasporto = dto.spesaTrasporto;
            entity.SpesaCibo = dto.spesaCibo;
            entity.SpesaAlloggio = dto.spesaAlloggio;

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Attività aggiornata con successo!" });
        }

        // ==========================
        // DELETE attività
        // ==========================
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

            // Verifica che non sia già inviata
            var rendicontazione = _ctx.RendicontazioniMensili
                .FirstOrDefault(r => r.DipendenteId == dip.Id && 
                                    r.Anno == entity.Giorno.Year && 
                                    r.Mese == entity.Giorno.Month);

            if (rendicontazione != null && rendicontazione.Stato != RendicontazioneStato.Bozza && rendicontazione.Stato != RendicontazioneStato.Respinta)
                return BadRequest(new { error = "Non puoi eliminare attività di una rendicontazione già inviata" });

            _ctx.AttivitaLavorative.Remove(entity);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Attività eliminata con successo!" });
        }

        // ==========================
        // GET progetti assegnati al dipendente
        // ==========================
        [HttpGet]
        public virtual IActionResult GetProgettiAssegnati()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var dip = _ctx.Dipendenti.FirstOrDefault(d => d.UserId.ToString() == userId);
            if (dip == null) return Unauthorized();

            var progettiAssegnati = _ctx.AssegnazioniDipendentiProgetti
                .Where(a => a.DipendenteId == dip.Id && a.Attivo)
                .Join(_ctx.Progetti,
                    assegnazione => assegnazione.ProgettoId,
                    progetto => progetto.Id,
                    (assegnazione, progetto) => new
                    {
                        id = progetto.Id,
                        nome = progetto.Nome,
                        cliente = progetto.Cliente,
                        completato = progetto.Completato,
                        label = $"{progetto.Nome} - {progetto.Cliente}"
                    })
                .Where(p => !p.completato)
                .OrderBy(p => p.nome)
                .ToList();

            return Ok(progettiAssegnati);
        }
    }

    // ==========================
    // DTO per le attività
    // ==========================
    public class AttivitaDTO
    {
        public int id { get; set; }
        public string giorno { get; set; }
        public string oraInizio { get; set; }
        public string oraFine { get; set; }
        public int progettoId { get; set; }  
        public string progetto { get; set; }  
        public string cliente { get; set; }
        public string attivita { get; set; }
        public string descrizione { get; set; }
        public bool trasferta { get; set; }
        public decimal spesaTrasporto { get; set; }
        public decimal spesaCibo { get; set; }
        public decimal spesaAlloggio { get; set; }
    }
}