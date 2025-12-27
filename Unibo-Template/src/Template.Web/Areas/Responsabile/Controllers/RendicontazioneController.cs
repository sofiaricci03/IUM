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

namespace Template.Web.Areas.Responsabile.Controllers
{
    [Area("Responsabile")]
    [Authorize(Roles = nameof(UserRole.Responsabile))]
    public partial class RendicontazioneController : Controller
    {
        private readonly TemplateDbContext _ctx;

        public RendicontazioneController(TemplateDbContext ctx)
        {
            _ctx = ctx;
        }

        public virtual IActionResult Index()
        {
            var nome = User.FindFirst(ClaimTypes.Name)?.Value ?? "Responsabile";
            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "N/A";

            var model = new RendicontazioneViewModel
            {
                NomeCompleto = nome,
                Email = email
            };

            return View(model);
        }

        // Stesso endpoint del Dipendente - il Responsabile rendiconta anche lui
        [HttpGet]
        public virtual async Task<IActionResult> GetStatoMese(int anno, int mese)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var dip = await _ctx.Dipendenti.FirstOrDefaultAsync(d => d.UserId.ToString() == userId);
            
            if (dip == null)
            {
                // Se il responsabile non ha un record Dipendente, ritorna vuoto
                return Json(new { giorni = new object[] { } });
            }

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

            var giorni = new System.Collections.Generic.List<object>();

            for (var data = primoGiorno; data <= ultimoGiorno; data = data.AddDays(1))
            {
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

            return Json(new { giorni });
        }
    }

    public class RendicontazioneViewModel
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
    }
}