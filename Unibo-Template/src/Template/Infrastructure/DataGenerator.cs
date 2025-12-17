using Template.Services.Shared;
using System;
using System.Linq;
using Template.Services;
using System.Security.Cryptography;
using System.Text;
using Template.Entities;

namespace Template.Infrastructure
{
    public class DataGenerator
    {
        public static void InitializeUsers(TemplateDbContext context)
        {
            if (context.Users.Any())
                return;   // Data already seeded

            string Hash(string password)
            {
                using var sha = SHA256.Create();
                return Convert.ToBase64String(
                    sha.ComputeHash(Encoding.ASCII.GetBytes(password))
                );
            }

            // ==============================
            // CREA GLI USER
            // ==============================

            var u1 = new User
            {
                Id = Guid.NewGuid(),
                Email = "dipendente1@azienda.it",
                Password = Hash("password1"),
                FirstName = "Luca",
                LastName = "Rossi",
                NickName = "dip1",
                Role = UserRole.Dipendente
            };

            var u2 = new User
            {
                Id = Guid.NewGuid(),
                Email = "dipendente2@azienda.it",
                Password = Hash("password2"),
                FirstName = "Marco",
                LastName = "Verdi",
                NickName = "dip2",
                Role = UserRole.Dipendente
            };

            var u3 = new User
            {
                Id = Guid.NewGuid(),
                Email = "dipendente3@azienda.it",
                Password = Hash("password3"),
                FirstName = "Giulia",
                LastName = "Bianchi",
                NickName = "dip3",
                Role = UserRole.Dipendente
            };

            var responsabile = new User
            {
                Id = Guid.NewGuid(),
                Email = "responsabile@azienda.it",
                Password = Hash("admin123"),
                FirstName = "Alessandro",
                LastName = "Ferrari",
                NickName = "boss",
                Role = UserRole.Responsabile
            };

            context.Users.AddRange(u1, u2, u3, responsabile);
            context.SaveChanges();

            // ==============================
            // CREA DIPENDENTI (collegati agli User)
            // ==============================

            var dip1 = context.Users.First(u => u.Email == "dipendente1@azienda.it");
            var dip2 = context.Users.First(u => u.Email == "dipendente2@azienda.it");
            var dip3 = context.Users.First(u => u.Email == "dipendente3@azienda.it");

            context.Dipendenti.AddRange(
                new Dipendente
                {
                    UserId = dip1.Id,
                    Nome = dip1.FirstName,
                    Cognome = dip1.LastName,
                    OreSettimanaliContratto = 40
                },
                new Dipendente
                {
                    UserId = dip2.Id,
                    Nome = dip2.FirstName,
                    Cognome = dip2.LastName,
                    OreSettimanaliContratto = 40
                },
                new Dipendente
                {
                    UserId = dip3.Id,
                    Nome = dip3.FirstName,
                    Cognome = dip3.LastName,
                    OreSettimanaliContratto = 40
                }
            );

            context.SaveChanges();
        }
    }
}
