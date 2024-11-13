using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Category - Movie (One-to-Many)
            modelBuilder.Entity<Domain.Entities.Movie>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Movies)
                .HasForeignKey(x => x.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
