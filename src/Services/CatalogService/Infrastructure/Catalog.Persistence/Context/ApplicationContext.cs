using Catalog.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieImage> MovieImages { get; set; }
        public DbSet<MovieDetail> MovieDetails { get; set; }
        public DbSet<MovieCast> MovieCasts { get; set; }
        public DbSet<MovieCrew> MovieCrews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category - Movie (One-to-Many)
            modelBuilder.Entity<Movie>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Movies)
                .HasForeignKey(x => x.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Movie - MovieImage (One-to-Many)
            modelBuilder.Entity<MovieImage>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MovieImages)
                .HasForeignKey(x => x.MovieID)
                .OnDelete(DeleteBehavior.Restrict);

            // Movie - MovieDetail (One-to-Many)
            modelBuilder.Entity<MovieDetail>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MovieDetails)
                .HasForeignKey(x => x.MovieID)
                .OnDelete(DeleteBehavior.Restrict);

            // Movie - MovieCast (One-to-Many)
            modelBuilder.Entity<MovieCast>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MovieCasts)
                .HasForeignKey(x => x.MovieID)
                .OnDelete(DeleteBehavior.Restrict);

            // Movie - MovieCrew (One-to-Many)
            modelBuilder.Entity<MovieCrew>()
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
