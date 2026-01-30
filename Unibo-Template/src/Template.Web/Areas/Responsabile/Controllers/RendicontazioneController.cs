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

namespace Template.Web.Areas.Responsabile.Controllers
{
    [Area("Responsabile")]
    [Authorize(Roles = nameof(UserRole.Responsabile))]
    public partial class RendicontazioneController : BaseAreaController
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
        // API: Ottiene lista rendicontazioni da approvare
        [HttpGet]
        public virtual async Task<IActionResult> GetRendicontazioniDaApprovare()
        {
            var rendicontazioni = await _ctx.RendicontazioniMensili
                .Where(r => r.Stato == RendicontazioneStato.Inviata)
                .OrderByDescending(r => r.DataInvio)
                .ToListAsync();

            var lista = new System.Collections.Generic.List<object>();

            foreach (var r in rendicontazioni)
            {
                var dipendente = await _ctx.Dipendenti.FindAsync(r.DipendenteId);
                
                lista.Add(new
                {
                    id = r.Id,
                    dipendenteId = r.DipendenteId,
                    dipendente = $"{dipendente.Nome} {dipendente.Cognome}",
                    anno = r.Anno,
                    mese = r.Mese,
                    meseNome = new DateTime(r.Anno, r.Mese, 1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("it-IT")),
                    dataInvio = r.DataInvio?.ToString("dd/MM/yyyy HH:mm"),
                    stato = r.Stato.ToString()
                });
            }

            return Json(new { rendicontazioni = lista });
        }

        // API: Dettaglio rendicontazione dipendente
        [HttpGet]
        public virtual async Task<IActionResult> GetDettaglioRendicontazione(int rendicontazioneId)
        {
            var rend = await _ctx.RendicontazioniMensili.FindAsync(rendicontazioneId);
            if (rend == null) return NotFound();

            var dipendente = await _ctx.Dipendenti.FindAsync(rend.DipendenteId);
            
            var primoGiorno = new DateTime(rend.Anno, rend.Mese, 1);
            var ultimoGiorno = primoGiorno.AddMonths(1).AddDays(-1);

            var attivita = await _ctx.AttivitaLavorative
                .Where(a => a.DipendenteId == rend.DipendenteId && 
                        a.Giorno >= primoGiorno && 
                        a.Giorno <= ultimoGiorno)
                .OrderBy(a => a.Giorno)
                .ToListAsync();

            var dettagli = attivita.Select(a => new
            {
                data = a.Giorno.ToString("dd/MM/yyyy"),
                oraInizio = a.OraInizio.ToString(@"hh\:mm"),
                oraFine = a.OraFine.ToString(@"hh\:mm"),
                ore = (a.OraFine - a.OraInizio).TotalHours,
                progetto = a.ProgettoNome,
                cliente = a.Cliente,
                attivita = a.Attivita,
                descrizione = a.Descrizione
            }).ToList();

            var oreTotali = attivita.Sum(a => (a.OraFine - a.OraInizio).TotalHours);

            return Json(new
            {
                dipendente = $"{dipendente.Nome} {dipendente.Cognome}",
                mese = new DateTime(rend.Anno, rend.Mese, 1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("it-IT")),
                dataInvio = rend.DataInvio?.ToString("dd/MM/yyyy HH:mm"),
                oreTotali = Math.Round(oreTotali, 1),
                dettagli = dettagli
            });
        }

        // API: Approva rendicontazione
        [HttpPost]
        public virtual async Task<IActionResult> Approva([FromBody] ApprovaRequest request)
        {
            var rend = await _ctx.RendicontazioniMensili.FindAsync(request.RendicontazioneId);
            if (rend == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var responsabile = await _ctx.Dipendenti.FirstOrDefaultAsync(d => d.UserId.ToString() == userId);

            rend.Stato = RendicontazioneStato.Approvata;
            rend.DataApprovazione = DateTime.Now;
            rend.ApprovatoDaResponsabileId = responsabile?.Id;
            rend.NoteResponsabile = request.Note;

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Rendicontazione approvata!" });
        }

        // API: Respingi rendicontazione
        [HttpPost]
        public virtual async Task<IActionResult> Respingi([FromBody] RespingiRequest request)
        {
            var rend = await _ctx.RendicontazioniMensili.FindAsync(request.RendicontazioneId);
            if (rend == null) return NotFound();

            if (string.IsNullOrWhiteSpace(request.Note))
                return BadRequest(new { error = "Le note sono obbligatorie per respingere" });

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var responsabile = await _ctx.Dipendenti.FirstOrDefaultAsync(d => d.UserId.ToString() == userId);

            rend.Stato = RendicontazioneStato.Respinta;
            rend.DataApprovazione = DateTime.Now;
            rend.ApprovatoDaResponsabileId = responsabile?.Id;
            rend.NoteResponsabile = request.Note;

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Rendicontazione respinta" });
        }
        
    }

    public class RendicontazioneViewModel
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
    }
    public class ApprovaRequest
    {
        public int RendicontazioneId { get; set; }
        public string Note { get; set; }
    }   

    public class RespingiRequest
    {
        public int RendicontazioneId { get; set; }
        public string Note { get; set; }
    }
}