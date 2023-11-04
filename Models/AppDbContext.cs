using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace AnnonceManager.Models
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // les cles etrangere de la classe candidature
            modelBuilder.Entity<Candidature>()
                .HasKey(x => new { x.CandidatId, x.OffreId });

            //1 entreprise creer plusieurs offres
            modelBuilder.Entity<Offre>()
            .HasOne(o => o.Entreprise)
            .WithMany(e => e.OffreCreer)
            .HasForeignKey(o => o.EntId)
            .IsRequired();
        }

        public DbSet<Offre> Offres { get; set; } 
        public DbSet<Candidat> Candidats { get; set; }
        public DbSet<Entreprise> Entreprises { get; set; }
    }
}
