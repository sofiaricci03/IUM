using Microsoft.EntityFrameworkCore;
using Template.Infrastructure;
using Template.Services.Shared;
using Template.Entities;

namespace Template.Services
{
    public class TemplateDbContext : DbContext
    {
        public TemplateDbContext()
        {
        }

        public TemplateDbContext(DbContextOptions<TemplateDbContext> options)
            : base(options)
        {
            // Seed utenti demo
            DataGenerator.InitializeUsers(this);
        }

        // Tabelle esistenti
        public DbSet<User> Users { get; set; }

        // Nuove entità
        public DbSet<Dipendente> Dipendenti { get; set; }
        public DbSet<Progetto> Progetti { get; set; }
        public DbSet<AttivitaLavorativa> AttivitaLavorative { get; set; }

        public DbSet<RichiestaFerie> RichiestaFerie { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // NON usare ToTable() perché il DB è InMemory

            // Relazioni elementari opzionali
            modelBuilder.Entity<AttivitaLavorativa>()
                .HasOne<Dipendente>()
                .WithMany()
                .HasForeignKey(a => a.DipendenteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AttivitaLavorativa>()
                .HasOne<Progetto>()
                .WithMany()
                .HasForeignKey(a => a.ProgettoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
