using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Template.Services;
using Template.Services.Shared;
using Template.Web.Areas;

namespace Template.Web.Areas.Responsabile.Controllers
{
    [Area("Responsabile")]
    [Authorize(Roles = nameof(UserRole.Responsabile))]
    public partial class CongedoController : BaseAreaController
    {
        private readonly TemplateDbContext _context;

        public CongedoController(TemplateDbContext context)
        {
            _context = context;
        }

        public virtual IActionResult Index()
        {
            var nome = User.FindFirst(ClaimTypes.Name)?.Value ?? "Responsabile";
            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "N/A";

            var model = new CongedoViewModel
            {
                NomeCompleto = nome,
                Email = email
            };

            return View(model);
        }
    }

    public class CongedoViewModel
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
    }
}