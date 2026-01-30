using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Template.Services.Shared;
using System.Security.Claims;
using Template.Services;
using System.Linq;

namespace Template.Web.Areas.Auth
{
    [Area("Auth")]
    public partial class AuthController : Controller
    {
        private readonly TemplateDbContext _context;

        public  AuthController(TemplateDbContext context)
        {
            _context = context;
        }

        public virtual IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public virtual IActionResult Login(string email, string password)
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
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
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

        public virtual  IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
