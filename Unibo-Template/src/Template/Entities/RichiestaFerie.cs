using System;

namespace Template.Entities
{
    public class RichiestaFerie
    {
        public int Id { get; set; }

        public int DipendenteId { get; set; }
        public Dipendente Dipendente { get; set; }

        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }

        public string Motivo { get; set; }
        public string Tipo { get; set; }  // ferie / permesso

        public FerieStato Stato { get; set; }

        public DateTime DataRichiesta { get; set; }
    }

    public enum FerieStato
    {
        InAttesa,
        Approvato,
        Rifiutato
    }
}
