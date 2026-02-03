using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Template.Entities
{
    /// <summary>
    /// Fattura emessa verso il cliente per un progetto
    /// </summary>
    public class Fattura
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Riferimento al progetto fatturato
        /// </summary>
        [Required]
        public int ProgettoId { get; set; }

        [ForeignKey(nameof(ProgettoId))]
        public virtual Progetto Progetto { get; set; }

        /// <summary>
        /// Numero fattura (es. "2026/001")
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string NumeroFattura { get; set; }

        /// <summary>
        /// Data emissione fattura
        /// </summary>
        [Required]
        public DateTime DataEmissione { get; set; }

        /// <summary>
        /// Periodo di riferimento - Data inizio
        /// </summary>
        public DateTime? PeriodoDa { get; set; }

        /// <summary>
        /// Periodo di riferimento - Data fine
        /// </summary>
        public DateTime? PeriodoA { get; set; }

        /// <summary>
        /// Costo orario applicato (€/ora)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CostoOrario { get; set; }

        /// <summary>
        /// Totale ore rendicontate sul progetto
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal OreTotali { get; set; }

        /// <summary>
        /// Importo totale (OreTotali * CostoOrario)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ImportoTotale { get; set; }

        /// <summary>
        /// Importo IVA (se applicabile)
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal ImportoIva { get; set; }

        /// <summary>
        /// Percentuale IVA applicata (es. 22%)
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal PercentualeIva { get; set; }

        /// <summary>
        /// Importo totale con IVA
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal ImportoTotaleConIva { get; set; }

        /// <summary>
        /// Stato della fattura
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Stato { get; set; } // Bozza, Inviata, Pagata

        /// <summary>
        /// Data di invio al cliente
        /// </summary>
        public DateTime? DataInvio { get; set; }

        /// <summary>
        /// Data pagamento (se pagata)
        /// </summary>
        public DateTime? DataPagamento { get; set; }

        /// <summary>
        /// Note aggiuntive sulla fattura
        /// </summary>
        [MaxLength(1000)]
        public string Note { get; set; }

        /// <summary>
        /// Responsabile che ha generato la fattura
        /// </summary>
        public int? ResponsabileId { get; set; }

        [ForeignKey(nameof(ResponsabileId))]
        public virtual Dipendente Responsabile { get; set; }
    }

    /// <summary>
    /// Stati possibili per una fattura
    /// </summary>
    public static class StatoFattura
    {
        public const string Bozza = "Bozza";
        public const string Inviata = "Inviata";
        public const string Pagata = "Pagata";
        public const string Annullata = "Annullata";
    }
}