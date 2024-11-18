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
        public DbSet<Domain.Entities.MovieDetail> MovieDetails { get; set; }
        public DbSet<Domain.Entities.MovieCast> MovieCasts { get; set; }
        public DbSet<Domain.Entities.MovieCrew> MovieCrews { get; set; }

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
                .OnDelete(DeleteBehavior.Restrict);

            // Movie - MovieDetail (One-to-Many)
            modelBuilder.Entity<Domain.Entities.MovieDetail>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MovieDetails)
                .HasForeignKey(x => x.MovieID)
                .OnDelete(DeleteBehavior.Restrict);

            // Movie - MovieCast (One-to-Many)
            modelBuilder.Entity<Domain.Entities.MovieCast>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MovieCasts)
                .HasForeignKey(x => x.MovieID)
                .OnDelete(DeleteBehavior.Restrict);

            // Movie - MovieCrew (One-to-Many)
            modelBuilder.Entity<Domain.Entities.MovieCrew>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MovieCrews)
                .HasForeignKey(x => x.MovieID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
