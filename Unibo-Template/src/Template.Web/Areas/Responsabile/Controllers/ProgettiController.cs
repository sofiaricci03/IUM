using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Template.Entities;
using Template.Services;
using Template.Services.Shared;
using Template.Web.Areas;
using System;

namespace Template.Web.Areas.Responsabile.Controllers
{
    [Area("Responsabile")]
    [Authorize(Roles = nameof(UserRole.Responsabile))]
    public partial class ProgettiController : BaseAreaController
    {
        private readonly TemplateDbContext _context;

        public ProgettiController(TemplateDbContext context)
        {
            _context = context;
        }

        public virtual async Task<IActionResult> Index()
        {
            var nome = User.FindFirst(ClaimTypes.Name)?.Value ?? "Responsabile";
            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "N/A";

            var progetti = await _context.Progetti
                .OrderByDescending(p => p.DataInizio)
                .ToListAsync();

            var model = new ProgettiIndexViewModel
            {
                NomeCompleto = nome,
                Email = email,
                Progetti = progetti
            };

            return View(model);
        }

        [HttpGet]
        public virtual async Task<IActionResult> Dettaglio(int id)
        {
            var progetto = await _context.Progetti.FindAsync(id);
            if (progetto == null)
                return NotFound();

            // Recupera tutti i dipendenti per l'assegnazione
            var dipendenti = await _context.Dipendenti.ToListAsync();

            var model = new ProgettoDettaglioViewModel
            {
                Progetto = progetto,
                Dipendenti = dipendenti
            };

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Crea([FromBody] ProgettoCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var progetto = new Progetto
            {
                Nome = request.Nome,
                Cliente = request.Cliente,
                Descrizione = request.Descrizione,
                DataInizio = request.DataInizio,
                DataScadenza = request.DataScadenza,
                ReferenteCliente = request.ReferenteCliente,
                ReferenteInterno = request.ReferenteInterno,
                Completato = false
            };

            _context.Progetti.Add(progetto);
            await _context.SaveChangesAsync();

            return Ok(new { id = progetto.Id });
        }

        [HttpPut]
        public virtual async Task<IActionResult> Aggiorna([FromBody] ProgettoUpdateRequest request)
        {
            var progetto = await _context.Progetti.FindAsync(request.Id);
            if (progetto == null)
                return NotFound();

            progetto.Nome = request.Nome;
            progetto.Cliente = request.Cliente;
            progetto.Descrizione = request.Descrizione;
            progetto.DataInizio = request.DataInizio;
            progetto.DataScadenza = request.DataScadenza;
            progetto.ReferenteCliente = request.ReferenteCliente;
            progetto.ReferenteInterno = request.ReferenteInterno;
            progetto.Completato = request.Completato;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Elimina(int id)
        {
            var progetto = await _context.Progetti.FindAsync(id);
            if (progetto == null)
                return NotFound();

            _context.Progetti.Remove(progetto);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public virtual async Task<IActionResult> Lista()
        {
            var progetti = await _context.Progetti
                .Select(p => new
                {
                    p.Id,
                    p.Nome,
                    p.Cliente,
                    p.Descrizione,
                    DataInizio = p.DataInizio.ToString("yyyy-MM-dd"),
                    DataScadenza = p.DataScadenza.ToString("yyyy-MM-dd"),
                    p.ReferenteCliente,
                    p.ReferenteInterno,
                    p.Completato
                })
                .ToListAsync();

            return Json(progetti);
        }
    }

    // ViewModels
    public class ProgettiIndexViewModel
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public List<Progetto> Progetti { get; set; }
    }

   public class ProgettoDettaglioViewModel
{
    public Progetto Progetto { get; set; }
    public List<Template.Entities.Dipendente> Dipendenti { get; set; }
}
    // Request Models
    public class ProgettoCreateRequest
    {
        public string Nome { get; set; }
        public string Cliente { get; set; }
        public string Descrizione { get; set; }
        public System.DateTime DataInizio { get; set; }
        public System.DateTime DataScadenza { get; set; }
        public string ReferenteCliente { get; set; }
        public string ReferenteInterno { get; set; }
    }

    public class ProgettoUpdateRequest
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cliente { get; set; }
        public string Descrizione { get; set; }
        public System.DateTime DataInizio { get; set; }
        public System.DateTime DataScadenza { get; set; }
        public string ReferenteCliente { get; set; }
        public string ReferenteInterno { get; set; }
        public bool Completato { get; set; }
    }
}