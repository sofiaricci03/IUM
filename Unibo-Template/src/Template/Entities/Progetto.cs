using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Template.Entities
{
    public class Progetto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cliente { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataScadenza { get; set; }
        public string ReferenteCliente { get; set; }
        public string ReferenteInterno { get; set; }
        public bool Completato { get; set; } = false;
    }
}
