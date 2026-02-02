namespace Template.Web.Areas.Dipendente.Models
{
    public class RichiestaFerieDTO
    {
        public int? id { get; set; } // Nullable per nuove richieste
        public string dal { get; set; }
        public string al { get; set; }
        public string motivo { get; set; }
        public string tipo { get; set; }
    }
}
