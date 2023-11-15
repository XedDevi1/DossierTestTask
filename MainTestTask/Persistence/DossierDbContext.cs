using MainTestTask.Models;
using Microsoft.EntityFrameworkCore;

namespace MainTestTask.Persistence
{
    public class DossierDbContext : DbContext
    {
        public DossierDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Dossier> Dossiers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dossier>()
                .HasOne(p => p.Parent)
                .WithMany(b => b.Children)
                .HasForeignKey(p => p.ParentId)
                .IsRequired(false) // Делаем ParentId необязательным
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
