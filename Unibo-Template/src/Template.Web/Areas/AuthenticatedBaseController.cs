using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using Template.Services;
using Template.Services.Shared;

namespace Template.Web.Areas.Auth
{
    [Area("Auth")]
    public class AuthController : Controller
    {
        private readonly TemplateDbContext _context;

        public AuthController(TemplateDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || !user.IsMatchWithPassword(password))
            {
                ViewBag.Error = "Credenziali non valide.";
                return View();
            }

            // ============================
            // CREA I CLAIM DELL'UTENTE
            // ============================
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            // Cookie di autenticazione
            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            // Redirect in base al ruolo
            if (user.Role == UserRole.Responsabile)
                return RedirectToAction("Dashboard", "Responsabile", new { area = "Responsabile" });

            return RedirectToAction("Dashboard", "Dipendente", new { area = "Dipendente" });
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
