using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Template.Services.Shared;

namespace Template.Web.Areas.Dipendente.Controllers
{
    [Area("Dipendente")]
    [Authorize(Roles = nameof(UserRole.Dipendente))]
    public partial class DashboardController : Controller
    {
        // IMPORTANTISSIMO: deve essere virtual per compatibilità con Sg4Mvc
        public virtual IActionResult Index()
        {
            // Recupero sicuro dei dati dal login (claims)
            var nome = User.FindFirst(ClaimTypes.Name)?.Value ?? "Utente";
            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "N/A";

            var model = new DipendenteDashboardViewModel
            {
                NomeCompleto = nome,
                Email = email
            };

            return View(model);
        }
    }

    public partial class DipendenteDashboardViewModel
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
    }
}
