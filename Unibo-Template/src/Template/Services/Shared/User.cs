using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Template.Services.Shared
{
    public enum UserRole
    {
        Dipendente = 0,
        Responsabile = 1
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }  // SHA256 Base64

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        /// <summary>
        /// Ruolo utente: Dipendente o Responsabile
        /// </summary>
        public UserRole Role { get; set; } = UserRole.Dipendente;

        /// <summary>
        /// Checks if password passed as parameter matches with the Password of the current user
        /// </summary>
        /// <param name="password">password to check</param>
        /// <returns>True if passwords match. False otherwise.</returns>
        public bool IsMatchWithPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;

            using var sha256 = SHA256.Create();
            var testPassword = Convert.ToBase64String(
                sha256.ComputeHash(Encoding.ASCII.GetBytes(password))
            );

            return this.Password == testPassword;
        }
    }
}
