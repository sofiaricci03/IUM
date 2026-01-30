using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Template.Entities
{
    public class Dipendente
    {
        public int Id { get; set; }

        // Collegamento con la tabella Users
        public Guid UserId { get; set; }

        public string Nome { get; set; }

        public string Cognome { get; set; }

        public int OreSettimanaliContratto { get; set; }

        // ferie residue, permessi, malattia
        public int FerieResidue { get; set; }
        public int PermessiResidui { get; set; }
        public int GiorniMalattiaDisponibili { get; set; }
        public byte[]? ProfileImage { get; set; }
        public string? ProfileImageContentType { get; set; }
    }
}