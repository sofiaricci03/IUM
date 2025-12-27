using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Web.Areas;

namespace Template.Web.Areas.Dipendente.Controllers
{
    [Area("Dipendente")]
    [Authorize(Roles = nameof(Template.Services.Shared.UserRole.Dipendente))]
    public partial class ProgettiController : BaseAreaController
    {
        public virtual IActionResult Index()
        {
            return View(); // Mostra la pagina divertente
        }
    }
}
