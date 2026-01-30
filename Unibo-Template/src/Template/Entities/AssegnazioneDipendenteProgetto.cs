using System;

namespace Template.Entities
{
    public class AssegnazioneDipendenteProgetto
    {
        public int Id { get; set; }
        public int DipendenteId { get; set; }
        public int ProgettoId { get; set; }
        public DateTime DataAssegnazione { get; set; }
        public DateTime? DataRimozione { get; set; }
        public bool Attivo { get; set; }
        
        // Navigation properties
        public Dipendente Dipendente { get; set; }
        public Progetto Progetto { get; set; }
    }
}