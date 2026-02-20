using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Template.Services;
using Template.Services.Shared;
using Template.Entities;
using Template.Web.Areas;

namespace Template.Web.Areas.Dipendente.Controllers
{
    [Area("Dipendente")]
    [Authorize(Roles = nameof(UserRole.Dipendente))]
    public partial class RendicontazioneController : BaseAreaController
    {
        private readonly TemplateDbContext _ctx;

        public RendicontazioneController(TemplateDbContext ctx)
        {
            _ctx = ctx;
        }

        public virtual IActionResult Index()
        {
            var nome = User.FindFirst(ClaimTypes.Name)?.Value ?? "Dipendente";
            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "N/A";

            var model = new RendicontazioneViewModel
            {
                NomeCompleto = nome,
                Email = email
            };

            return View(model);
        }

        // API per ottenere lo stato del mese con colori rosso/verde
        [HttpGet]
        public virtual async Task<IActionResult> GetStatoMese(int anno, int mese)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var dip = await _ctx.Dipendenti.FirstOrDefaultAsync(d => d.UserId.ToString() == userId);
            
            if (dip == null) return Unauthorized();

            var primoGiorno = new DateTime(anno, mese, 1);
            var ultimoGiorno = primoGiorno.AddMonths(1).AddDays(-1);

            // Recupera attività del mese
            var attivita = await _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == dip.Id && 
                           a.Giorno >= primoGiorno && 
                           a.Giorno <= ultimoGiorno)
                .ToListAsync();

            // Recupera ferie/permessi approvati del mese
            var congedi = await _ctx.RichiestaFerie
                .Where(r => r.DipendenteId == dip.Id &&
                           r.Stato == FerieStato.Approvato &&
                           ((r.DataInizio <= ultimoGiorno && r.DataFine >= primoGiorno)))
                .ToListAsync();

            // Recupera stato rendicontazione
            var rendicontazione = await _ctx.RendicontazioniMensili
                .FirstOrDefaultAsync(r => r.DipendenteId == dip.Id && r.Anno == anno && r.Mese == mese);

            var giorni = new System.Collections.Generic.List<object>();

            for (var data = primoGiorno; data <= ultimoGiorno; data = data.AddDays(1))
            {
                // Salta weekend
                if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                var attivitaGiorno = attivita.Where(a => a.Giorno.Date == data.Date).ToList();
                var inCongedo = congedi.Any(c => data.Date >= c.DataInizio.Date && data.Date <= c.DataFine.Date);

                string stato;
                string colorClass;
                
                if (inCongedo)
                {
                    stato = "Congedo";
                    colorClass = "status-congedo";
                }
                else if (attivitaGiorno.Any())
                {
                    var oreTotali = attivitaGiorno.Sum(a => (a.OraFine - a.OraInizio).TotalHours);
                    
                    if (oreTotali >= 6)
                    {
                        stato = "Completo";
                        colorClass = "status-completo";
                    }
                    else
                    {
                        stato = "Parziale";
                        colorClass = "status-parziale";
                    }
                }
                else
                {
                    stato = "Mancante";
                    colorClass = "status-mancante";
                }

                var ore = attivitaGiorno.Sum(a => (a.OraFine - a.OraInizio).TotalHours);

                giorni.Add(new
                {
                    data = data.ToString("yyyy-MM-dd"),
                    dataFormattata = data.ToString("dd/MM"),
                    giornoSettimana = data.ToString("ddd", new System.Globalization.CultureInfo("it-IT")),
                    stato = stato,
                    colorClass = colorClass,
                    ore = Math.Round(ore, 1),
                    numeroAttivita = attivitaGiorno.Count
                });
            }

            return Json(new { 
                giorni = giorni,
                statoRendicontazione = rendicontazione?.Stato.ToString() ?? "Bozza",
                dataInvio = rendicontazione?.DataInvio?.ToString("dd/MM/yyyy"),
                dataApprovazione = rendicontazione?.DataApprovazione?.ToString("dd/MM/yyyy"),
                noteResponsabile = rendicontazione?.NoteResponsabile
            });
        }

        // Invia rendicontazione per approvazione
        [HttpPost]
        public virtual async Task<IActionResult> InviaRendicontazione(int anno, int mese)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var dip = await _ctx.Dipendenti.FirstOrDefaultAsync(d => d.UserId.ToString() == userId);
            
            if (dip == null) return Unauthorized();

            // Verifica che non ci siano giorni mancanti
            var primoGiorno = new DateTime(anno, mese, 1);
            var ultimoGiorno = primoGiorno.AddMonths(1).AddDays(-1);

            var attivita = await _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == dip.Id && 
                           a.Giorno >= primoGiorno && 
                           a.Giorno <= ultimoGiorno)
                .ToListAsync();

            var congedi = await _ctx.RichiestaFerie
                .Where(r => r.DipendenteId == dip.Id &&
                           r.Stato == FerieStato.Approvato &&
                           ((r.DataInizio <= ultimoGiorno && r.DataFine >= primoGiorno)))
                .ToListAsync();

            // Conta giorni lavorativi mancanti
            int giorniMancanti = 0;
            for (var data = primoGiorno; data <= ultimoGiorno; data = data.AddDays(1))
            {
                if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                var attivitaGiorno = attivita.Any(a => a.Giorno.Date == data.Date);
                var inCongedo = congedi.Any(c => data.Date >= c.DataInizio.Date && data.Date <= c.DataFine.Date);

                if (!attivitaGiorno && !inCongedo)
                    giorniMancanti++;
            }

            if (giorniMancanti > 0)
            {
                return BadRequest(new { error = $"Ci sono ancora {giorniMancanti} giorni lavorativi senza rendicontazione!" });
            }

            // Crea o aggiorna rendicontazione
            var rendicontazione = await _ctx.RendicontazioniMensili
                .FirstOrDefaultAsync(r => r.DipendenteId == dip.Id && r.Anno == anno && r.Mese == mese);

            if (rendicontazione == null)
            {
                rendicontazione = new RendicontazioneMensile
                {
                    DipendenteId = dip.Id,
                    Anno = anno,
                    Mese = mese,
                    DataCreazione = DateTime.Now
                };
                _ctx.RendicontazioniMensili.Add(rendicontazione);
            }

            rendicontazione.Stato = RendicontazioneStato.Inviata;
            rendicontazione.DataInvio = DateTime.Now;
            rendicontazione.NoteResponsabile = null; // Reset note se reinviata

            await _ctx.SaveChangesAsync();

        return NoContent();
        }

        // API per ottenere progetti disponibili
        [HttpGet]
        public virtual async Task<IActionResult> GetProgettiDisponibili()
        {
            var progetti = await _ctx.Progetti
                .Where(p => !p.Completato)
                .Select(p => new
                {
                    p.Id,
                    p.Nome,
                    p.Cliente,
                    label = $"{p.Nome} - {p.Cliente}"
                })
                .ToListAsync();

            return Json(progetti);
        }
    }

    public class RendicontazioneViewModel
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
    }
}