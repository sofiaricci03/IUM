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

namespace Template.Web.Areas.Dipendente.Controllers
{
    [Area("Dipendente")]
    [Authorize(Roles = nameof(UserRole.Dipendente))]
    public partial class ProgettiController : BaseAreaController
    {
        private readonly TemplateDbContext _context;

        public ProgettiController(TemplateDbContext context)
        {
            _context = context;
        }

        public virtual async Task<IActionResult> Index()
        {
            var nome = User.FindFirst(ClaimTypes.Name)?.Value ?? "Dipendente";
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
    }

    public class ProgettiIndexViewModel
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public List<Progetto> Progetti { get; set; }
    }
}