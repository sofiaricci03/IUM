using System;

namespace Template.Entities
{
    public class RendicontazioneMensile
    {
        public int Id { get; set; }
        public int DipendenteId { get; set; }
        public int Anno { get; set; }
        public int Mese { get; set; }
        public RendicontazioneStato Stato { get; set; }
        public DateTime? DataInvio { get; set; }
        public DateTime? DataApprovazione { get; set; }
        public int? ApprovatoDaResponsabileId { get; set; }
        public string NoteResponsabile { get; set; }
        public DateTime DataCreazione { get; set; }
    }

    public enum RendicontazioneStato
    {
        Bozza = 0,           // Dipendente sta compilando
        Inviata = 1,         // Inviata, in attesa approvazione
        Approvata = 2,       // OK, va a payroll
        Respinta = 3         // Respinta, torna al dipendente
    }
}