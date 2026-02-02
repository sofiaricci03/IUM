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
            DataGenerator.InitializeUsers(this);
        }
        // Tabelle esistenti
        public DbSet<User> Users { get; set; }

        // Nuove entità
        public DbSet<Dipendente> Dipendenti { get; set; }
        public DbSet<Progetto> Progetti { get; set; }
        public DbSet<AttivitaLavorativa> AttivitaLavorative { get; set; }
        public DbSet<RichiestaFerie> RichiestaFerie { get; set; }
        public DbSet<RendicontazioneMensile> RendicontazioniMensili { get; set; }
        public DbSet<AssegnazioneDipendenteProgetto> AssegnazioniDipendentiProgetti { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Non si usa ToTable() perché il DB è InMemory

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
            
             modelBuilder.Entity<AssegnazioneDipendenteProgetto>()
                .HasOne(a => a.Dipendente)
                .WithMany()
                .HasForeignKey(a => a.DipendenteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssegnazioneDipendenteProgetto>()
                .HasOne(a => a.Progetto)
                .WithMany()
                .HasForeignKey(a => a.ProgettoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        
    }
}
