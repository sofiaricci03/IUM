using Template.Services.Shared;
using System;
using System.Linq;
using Template.Services;
using System.Security.Cryptography;
using System.Text;
using Template.Entities;
using System.Collections.Generic;

namespace Template.Infrastructure
{
    public class DataGenerator
    {
        private static readonly object _lock = new object();
        private static bool _initialized = false;

        public static void InitializeUsers(TemplateDbContext context)
        {
            // Thread-safe check
            lock (_lock)
            {
                if (_initialized || context.Users.Any())
                    return;

                _initialized = true;
            }

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
            // CREA DIPENDENTI 
            // ==============================

            var dip1 = context.Users.First(u => u.Email == "dipendente1@azienda.it");
            var dip2 = context.Users.First(u => u.Email == "dipendente2@azienda.it");
            var dip3 = context.Users.First(u => u.Email == "dipendente3@azienda.it");
            var responsabileUser = context.Users.First(u => u.Email == "responsabile@azienda.it");

            var dipendenti = new List<Dipendente>
            {
                new Dipendente
                {
                    Id = 1,
                    UserId = dip1.Id,
                    Nome = dip1.FirstName,
                    Cognome = dip1.LastName,
                    OreSettimanaliContratto = 40
                },
                new Dipendente
                {
                    Id = 2,
                    UserId = dip2.Id,
                    Nome = dip2.FirstName,
                    Cognome = dip2.LastName,
                    OreSettimanaliContratto = 40
                },
                new Dipendente
                {
                    Id = 3,
                    UserId = dip3.Id,
                    Nome = dip3.FirstName,
                    Cognome = dip3.LastName,
                    OreSettimanaliContratto = 40
                },
                new Dipendente
                {
                    Id = 4,
                    UserId = responsabileUser.Id,
                    Nome = responsabileUser.FirstName,
                    Cognome = responsabileUser.LastName,
                    OreSettimanaliContratto = 40
                }
            };

            context.Dipendenti.AddRange(dipendenti);
            context.SaveChanges();

            // ==============================
            // CREA PROGETTI
            // ==============================

            var progetti = new List<Progetto>
            {
                new Progetto
                {
                    Id = 1,
                    Nome = "Sviluppo ERP",
                    Cliente = "Acme Corp",
                    Descrizione = "Sistema gestionale integrato per PMI",
                    DataInizio = new DateTime(2024, 1, 10),
                    DataScadenza = new DateTime(2025, 6, 30),
                    Completato = false,
                    ReferenteInterno = "Alessandro Ferrari",
                    ReferenteCliente = "John Smith"
                },
                new Progetto
                {
                    Id = 2,
                    Nome = "Portale E-commerce",
                    Cliente = "Fashion Store",
                    Descrizione = "Piattaforma vendita online con gestione magazzino",
                    DataInizio = new DateTime(2024, 3, 1),
                    DataScadenza = new DateTime(2025, 12, 31),
                    Completato = false,
                    ReferenteInterno = "Alessandro Ferrari",
                    ReferenteCliente = "Anna Neri"
                },
                new Progetto
                {
                    Id = 3,
                    Nome = "App Mobile Banking",
                    Cliente = "Banca Nazionale",
                    Descrizione = "Applicazione mobile per servizi bancari",
                    DataInizio = new DateTime(2024, 2, 15),
                    DataScadenza = new DateTime(2025, 8, 15),
                    Completato = false,
                    ReferenteInterno = "Alessandro Ferrari",
                    ReferenteCliente = "Marco Blu"
                },
                new Progetto
                {
                    Id = 4,
                    Nome = "Sito Web Aziendale",
                    Cliente = "Tech Solutions",
                    Descrizione = "Rifacimento completo sito corporate",
                    DataInizio = new DateTime(2024, 4, 1),
                    DataScadenza = new DateTime(2025, 10, 31),
                    Completato = false,
                    ReferenteInterno = "Alessandro Ferrari",
                    ReferenteCliente = "Sara Gialli"
                }
            };

            context.Progetti.AddRange(progetti);
            context.SaveChanges();

            // ==============================
            // ASSEGNAZIONI DIPENDENTE-PROGETTO
            // ==============================

            var assegnazioni = new List<AssegnazioneDipendenteProgetto>
            {
                
                new AssegnazioneDipendenteProgetto
                {
                    Id = 1,
                    DipendenteId = 1,
                    ProgettoId = 1,
                    DataAssegnazione = new DateTime(2024, 1, 10),
                    Attivo = true
                },
                new AssegnazioneDipendenteProgetto
                {
                    Id = 2,
                    DipendenteId = 1,
                    ProgettoId = 2,
                    DataAssegnazione = new DateTime(2024, 3, 1),
                    Attivo = true
                },
                
                
                new AssegnazioneDipendenteProgetto
                {
                    Id = 3,
                    DipendenteId = 2,
                    ProgettoId = 2,
                    DataAssegnazione = new DateTime(2024, 3, 1),
                    Attivo = true
                },
                new AssegnazioneDipendenteProgetto
                {
                    Id = 4,
                    DipendenteId = 2,
                    ProgettoId = 3,
                    DataAssegnazione = new DateTime(2024, 2, 15),
                    Attivo = true
                },
                
                new AssegnazioneDipendenteProgetto
                {
                    Id = 5,
                    DipendenteId = 3,
                    ProgettoId = 3,
                    DataAssegnazione = new DateTime(2024, 2, 15),
                    Attivo = true
                },
                new AssegnazioneDipendenteProgetto
                {
                    Id = 6,
                    DipendenteId = 3,
                    ProgettoId = 4,
                    DataAssegnazione = new DateTime(2024, 4, 1),
                    Attivo = true
                },
                
                new AssegnazioneDipendenteProgetto
                {
                    Id = 7,
                    DipendenteId = 4,
                    ProgettoId = 1,
                    DataAssegnazione = new DateTime(2024, 1, 10),
                    Attivo = true
                },
                new AssegnazioneDipendenteProgetto
                {
                    Id = 8,
                    DipendenteId = 4,
                    ProgettoId = 2,
                    DataAssegnazione = new DateTime(2024, 3, 1),
                    Attivo = true
                },
                new AssegnazioneDipendenteProgetto
                {
                    Id = 9,
                    DipendenteId = 4,
                    ProgettoId = 3,
                    DataAssegnazione = new DateTime(2024, 2, 15),
                    Attivo = true
                },
                new AssegnazioneDipendenteProgetto
                {
                    Id = 10,
                    DipendenteId = 4,
                    ProgettoId = 4,
                    DataAssegnazione = new DateTime(2024, 4, 1),
                    Attivo = true
                }
            };

            context.AssegnazioniDipendentiProgetti.AddRange(assegnazioni);
            context.SaveChanges();
        }
    }
}