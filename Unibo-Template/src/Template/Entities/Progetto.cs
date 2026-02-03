using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Template.Entities
{
    public class Progetto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nome { get; set; }

        [MaxLength(200)]
        public string Cliente { get; set; }

        [MaxLength(1000)]
        public string Descrizione { get; set; }

        public DateTime DataInizio { get; set; }

        public DateTime DataScadenza { get; set; }

        [MaxLength(100)]
        public string ReferenteCliente { get; set; }

        [MaxLength(100)]
        public string ReferenteInterno { get; set; }

        public bool Completato { get; set; } = false;

        // ===== RELAZIONI =====
        
        /// <summary>
        /// Attività lavorative svolte su questo progetto
        /// </summary>
        public virtual ICollection<AttivitaLavorativa> AttivitaLavorative { get; set; } = new List<AttivitaLavorativa>();

        /// <summary>
        /// Assegnazioni dipendenti a questo progetto
        /// </summary>
        public virtual ICollection<AssegnazioneDipendenteProgetto> AssegnazioniDipendenti { get; set; } = new List<AssegnazioneDipendenteProgetto>();

        /// <summary>
        /// Fatture emesse per questo progetto
        /// </summary>
        public virtual ICollection<Fattura> Fatture { get; set; } = new List<Fattura>();
    }
}