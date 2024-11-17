using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Category> Categories { get; set; }
        public DbSet<Domain.Entities.Movie> Movies { get; set; }
        public DbSet<Domain.Entities.MovieImage> MovieImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category - Movie (One-to-Many)
            modelBuilder.Entity<Domain.Entities.Movie>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Movies)
                .HasForeignKey(x => x.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Movie - MovieImage (One-to-Many)
            modelBuilder.Entity<Domain.Entities.MovieImage>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MovieImages)
                .HasForeignKey(x => x.MovieID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
