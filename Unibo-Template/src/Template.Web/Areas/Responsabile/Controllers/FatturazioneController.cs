using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template.Entities;
using Template.Services;

namespace Template.Web.Areas.Responsabile.Controllers
{
    [Area("Responsabile")]
    [Authorize(Roles = "Responsabile")]

    public partial class FatturazioneController : Controller
    {
        private readonly TemplateDbContext _context;

        public FatturazioneController(TemplateDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Vista principale
        /// </summary>
        public virtual async Task<IActionResult> Index()
        {
            return View();
        }

        /// <summary>
        /// API: Lista progetti con ore rendicontate
        /// </summary>
        [HttpGet]
        public virtual async Task<IActionResult> GetProgetti()
        {
            var progetti = await _context.Progetti
                .Where(p => !p.Completato)
                .Select(p => new
                {
                    p.Id,
                    p.Nome,
                    p.Cliente,
                    p.DataInizio,
                    p.DataScadenza,
                    OreTotali = _context.AttivitaLavorative
                        .Where(a => a.ProgettoId == p.Id)
                        .Where(a => _context.RendicontazioniMensili
                            .Any(r => r.DipendenteId == a.DipendenteId &&
                                      r.Anno == a.Giorno.Year &&
                                      r.Mese == a.Giorno.Month &&
                                      r.Stato == Template.Entities.RendicontazioneStato.Approvata))
                        .Sum(a => (decimal?)((a.OraFine - a.OraInizio).TotalHours)) ?? 0,
                    NumeroFatture = _context.Fatture.Count(f => f.ProgettoId == p.Id),
                    UltimaFattura = _context.Fatture
                        .Where(f => f.ProgettoId == p.Id)
                        .OrderByDescending(f => f.DataEmissione)
                        .Select(f => new { f.NumeroFattura, f.DataEmissione })
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Json(progetti);
        }

        /// <summary>
        /// API: Ottieni dettaglio ore per progetto
        /// </summary>
        [HttpGet]
        public virtual async Task<IActionResult> GetDettaglioProgetto(int progettoId)
        {
            var progetto = await _context.Progetti
                .FirstOrDefaultAsync(p => p.Id == progettoId);

            if (progetto == null)
                return NotFound(new { error = "Progetto non trovato" });

            // Query diretta per attività approvate con JOIN su Dipendenti
            var attivitaQuery = from a in _context.AttivitaLavorative
                               join d in _context.Dipendenti on a.DipendenteId equals d.Id
                               where a.ProgettoId == progettoId
                               where _context.RendicontazioniMensili
                                   .Any(r => r.DipendenteId == a.DipendenteId &&
                                             r.Anno == a.Giorno.Year &&
                                             r.Mese == a.Giorno.Month &&
                                             r.Stato == Template.Entities.RendicontazioneStato.Approvata)
                               select new
                               {
                                   a.Id,
                                   a.DipendenteId,
                                   DipendenteNome = d.Nome,
                                   DipendenteCognome = d.Cognome,
                                   a.Giorno,
                                   a.OraInizio,
                                   a.OraFine
                               };

            var attivita = await attivitaQuery.ToListAsync();

            var dettaglioDipendenti = attivita
                .GroupBy(a => new { a.DipendenteId, a.DipendenteNome, a.DipendenteCognome })
                .Select(g => new
                {
                    DipendenteId = g.Key.DipendenteId,
                    NomeDipendente = $"{g.Key.DipendenteNome} {g.Key.DipendenteCognome}",
                    OreTotali = g.Sum(a => (decimal)((a.OraFine - a.OraInizio).TotalHours)),
                    NumeroAttivita = g.Count(),
                    PeriodoDa = g.Min(a => a.Giorno),
                    PeriodoA = g.Max(a => a.Giorno)
                })
                .ToList();

            var oreTotali = dettaglioDipendenti.Sum(d => d.OreTotali);

            var result = new
            {
                ProgettoId = progetto.Id,
                NomeProgetto = progetto.Nome,
                Cliente = progetto.Cliente,
                OreTotali = oreTotali,
                DettaglioDipendenti = dettaglioDipendenti,
                PeriodoDa = attivita.Any() ? attivita.Min(a => a.Giorno) : (DateTime?)null,
                PeriodoA = attivita.Any() ? attivita.Max(a => a.Giorno) : (DateTime?)null
            };

            return Json(result);
        }

        /// <summary>
        /// API: Genera preview fattura
        /// </summary>
        [HttpPost]
        public virtual async Task<IActionResult> GeneraPreview([FromBody] PreviewRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var progetto = await _context.Progetti
                .FirstOrDefaultAsync(p => p.Id == request.ProgettoId);

            if (progetto == null)
                return NotFound(new { error = "Progetto non trovato" });

            // Calcola ore totali (solo attività approvate)
            var attivita = await _context.AttivitaLavorative
                .Where(a => a.ProgettoId == request.ProgettoId)
                .Where(a => _context.RendicontazioniMensili
                    .Any(r => r.DipendenteId == a.DipendenteId &&
                              r.Anno == a.Giorno.Year &&
                              r.Mese == a.Giorno.Month &&
                              r.Stato == Template.Entities.RendicontazioneStato.Approvata))
                .ToListAsync();

            if (!attivita.Any())
            {
                return BadRequest(new { error = "Nessuna attività approvata trovata per questo progetto" });
            }

            var oreTotali = attivita.Sum(a => (decimal)((a.OraFine - a.OraInizio).TotalHours));

            // Calcola importi
            var importoImponibile = oreTotali * request.CostoOrario;
            var importoIva = importoImponibile * (request.PercentualeIva / 100);
            var importoTotale = importoImponibile + importoIva;

            // Genera numero fattura progressivo
            var anno = DateTime.Now.Year;
            var ultimaFattura = await _context.Fatture
                .Where(f => f.NumeroFattura.StartsWith($"{anno}/"))
                .OrderByDescending(f => f.NumeroFattura)
                .FirstOrDefaultAsync();

            int progressivo = 1;
            if (ultimaFattura != null)
            {
                var parts = ultimaFattura.NumeroFattura.Split('/');
                if (parts.Length == 2 && int.TryParse(parts[1], out int numero))
                    progressivo = numero + 1;
            }

            var numeroFattura = $"{anno}/{progressivo:D3}";

            var preview = new
            {
                NumeroFattura = numeroFattura,
                DataEmissione = DateTime.Now,
                Cliente = progetto.Cliente,
                NomeProgetto = progetto.Nome,
                PeriodoDa = attivita.Min(a => a.Giorno),
                PeriodoA = attivita.Max(a => a.Giorno),
                OreTotali = oreTotali,
                CostoOrario = request.CostoOrario,
                ImportoImponibile = Math.Round(importoImponibile, 2),
                PercentualeIva = request.PercentualeIva,
                ImportoIva = Math.Round(importoIva, 2),
                ImportoTotale = Math.Round(importoTotale, 2),
                Note = request.Note
            };

            return Json(preview);
        }

        /// <summary>
        /// API: Invia fattura al cliente
        /// </summary>
        [HttpPost]
        public virtual async Task<IActionResult> InviaFattura([FromBody] InviaFatturaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var progetto = await _context.Progetti
                .FirstOrDefaultAsync(p => p.Id == request.ProgettoId);

            if (progetto == null)
                return NotFound(new { error = "Progetto non trovato" });

            // Ottieni dipendente responsabile
            var userEmail = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            var dipendente = await _context.Dipendenti.FirstOrDefaultAsync(d => d.UserId == user.Id);

            // Calcola ore totali (solo attività approvate)
            var attivita = await _context.AttivitaLavorative
                .Where(a => a.ProgettoId == request.ProgettoId)
                .Where(a => _context.RendicontazioniMensili
                    .Any(r => r.DipendenteId == a.DipendenteId &&
                              r.Anno == a.Giorno.Year &&
                              r.Mese == a.Giorno.Month &&
                              r.Stato == Template.Entities.RendicontazioneStato.Approvata))
                .ToListAsync();

            if (!attivita.Any())
            {
                return BadRequest(new { error = "Nessuna attività approvata trovata per questo progetto" });
            }

            var oreTotali = attivita.Sum(a => (decimal)((a.OraFine - a.OraInizio).TotalHours));

            // Calcola importi
            var importoImponibile = oreTotali * request.CostoOrario;
            var importoIva = importoImponibile * (request.PercentualeIva / 100);
            var importoTotale = importoImponibile + importoIva;

            // Genera numero fattura
            var anno = DateTime.Now.Year;
            var ultimaFattura = await _context.Fatture
                .Where(f => f.NumeroFattura.StartsWith($"{anno}/"))
                .OrderByDescending(f => f.NumeroFattura)
                .FirstOrDefaultAsync();

            int progressivo = 1;
            if (ultimaFattura != null)
            {
                var parts = ultimaFattura.NumeroFattura.Split('/');
                if (parts.Length == 2 && int.TryParse(parts[1], out int numero))
                    progressivo = numero + 1;
            }

            var numeroFattura = $"{anno}/{progressivo:D3}";

            // Crea fattura
            var fattura = new Fattura
            {
                ProgettoId = progetto.Id,
                NumeroFattura = numeroFattura,
                DataEmissione = DateTime.Now,
                PeriodoDa = attivita.Min(a => a.Giorno),
                PeriodoA = attivita.Max(a => a.Giorno),
                CostoOrario = request.CostoOrario,
                OreTotali = oreTotali,
                ImportoTotale = Math.Round(importoImponibile, 2),
                PercentualeIva = request.PercentualeIva,
                ImportoIva = Math.Round(importoIva, 2),
                ImportoTotaleConIva = Math.Round(importoTotale, 2),
                Stato = "Inviata",
                DataInvio = DateTime.Now,
                Note = request.Note,
                ResponsabileId = dipendente?.Id
            };

            _context.Fatture.Add(fattura);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = $"Fattura {numeroFattura} inviata con successo al cliente {progetto.Cliente}",
                fatturaId = fattura.Id,
                numeroFattura = fattura.NumeroFattura
            });
        }

        /// <summary>
        /// API: Lista fatture emesse
        /// </summary>
        [HttpGet]
        public virtual async Task<IActionResult> GetFatture()
        {
            var fatture = await (from f in _context.Fatture
                                 join p in _context.Progetti on f.ProgettoId equals p.Id
                                 join d in _context.Dipendenti on f.ResponsabileId equals d.Id into dipJoin
                                 from d in dipJoin.DefaultIfEmpty()
                                 orderby f.DataEmissione descending
                                 select new
                                 {
                                     f.Id,
                                     f.NumeroFattura,
                                     f.DataEmissione,
                                     Cliente = p.Cliente,
                                     NomeProgetto = p.Nome,
                                     f.OreTotali,
                                     f.CostoOrario,
                                     f.ImportoTotaleConIva,
                                     f.Stato,
                                     f.DataInvio,
                                     f.DataPagamento,
                                     Responsabile = d != null ? $"{d.Nome} {d.Cognome}" : "N/D"
                                 })
                                 .ToListAsync();

            return Json(fatture);
        }
    }

    // ===== REQUEST MODELS =====

    public class PreviewRequest
    {
        public int ProgettoId { get; set; }
        public decimal CostoOrario { get; set; }
        public decimal PercentualeIva { get; set; } = 22;
        public string Note { get; set; }
    }

    public class InviaFatturaRequest
    {
        public int ProgettoId { get; set; }
        public decimal CostoOrario { get; set; }
        public decimal PercentualeIva { get; set; } = 22;
        public string Note { get; set; }
    }
}