using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Template.Entities
{
    public class AttivitaLavorativa
    {
        public int Id { get; set; }

        public int DipendenteId { get; set; }
        public int ProgettoId { get; set; }

        public string Cliente { get; set; }
        public string ProgettoNome { get; set; }

        public DateTime Giorno { get; set; }

        public TimeSpan OraInizio { get; set; }
        public TimeSpan OraFine { get; set; }

        public string Attivita { get; set; }
        public string Descrizione { get; set; }

        // Trasferta
        public bool Trasferta { get; set; }
        public decimal SpesaTrasporto { get; set; }
        public decimal SpesaCibo { get; set; }
        public decimal SpesaAlloggio { get; set; }
    }
}
