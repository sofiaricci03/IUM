using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Template.Services.Shared;

namespace Template.Web.Areas.Responsabile.Controllers
{
    [Area("Responsabile")]
    [Authorize(Roles = nameof(UserRole.Responsabile))]
    public partial class DashboardController : Controller
    {
        public virtual IActionResult Index()
        {
            var nome = User.FindFirst(ClaimTypes.Name)?.Value ?? "Responsabile";
            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "N/A";

            var model = new ResponsabileDashboardViewModel
            {
                NomeCompleto = nome,
                Email = email
            };

            return View(model);
        }
    }

    public class ResponsabileDashboardViewModel
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
    }
}
