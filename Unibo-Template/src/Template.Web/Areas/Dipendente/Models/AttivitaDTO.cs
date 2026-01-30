namespace Template.Web.Areas.Dipendente.Models
{
    public class AttivitaDTO
    {   
        public int id { get; set; }
        public string giorno { get; set; }
        public string oraInizio { get; set; }
        public string oraFine { get; set; }
        public string progetto { get; set; }
        public string cliente { get; set; }
        public string attivita { get; set; }
        public string descrizione { get; set; }
        public bool trasferta { get; set; }
        public decimal spesaTrasporto { get; set; }
        public decimal spesaCibo { get; set; }
        public decimal spesaAlloggio { get; set; }
    }
}