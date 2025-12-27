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

namespace Template.Web.Areas.Dipendente.Controllers
{
    [Area("Dipendente")]
    [Authorize(Roles = nameof(UserRole.Dipendente))]
    public partial class RendicontazioneController : Controller
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
                    // Calcola ore totali
                    var oreTotali = attivitaGiorno.Sum(a => (a.OraFine - a.OraInizio).TotalHours);
                    
                    // Verde se almeno 6 ore (giornata tipo), altrimenti arancione
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
                    // Rosso se manca completamente
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

            return Json(new { giorni });
        }

        // API per ottenere progetti disponibili (per rendicontazione veloce)
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