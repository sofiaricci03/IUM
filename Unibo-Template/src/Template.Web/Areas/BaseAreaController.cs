using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Template.Web.Areas
{
    public partial class BaseAreaController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            
            // Popola IdentitaViewModel automaticamente per tutte le aree
            if (User.Identity.IsAuthenticated)
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
                var nomeCompleto = User.FindFirst(ClaimTypes.Name)?.Value ?? email;
                
                var identita = new IdentitaViewModel
                {
                    EmailUtenteCorrente = email,
                    NomeCompletoUtenteCorrente = nomeCompleto
                    // GravatarUrl è calcolato automaticamente dalla property
                };
                
                ViewData[IdentitaViewModel.VIEWDATA_IDENTITACORRENTE_KEY] = identita;
            }
        }
    }
}