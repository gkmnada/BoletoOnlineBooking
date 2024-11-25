using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ticket.Domain.Entities;

namespace Ticket.Persistence.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Domain.Entities.Ticket> Tickets { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cinema - City (One-to-Many)
            modelBuilder.Entity<Cinema>()
                .HasOne(x => x.City)
                .WithMany(x => x.Cinemas)
                .HasForeignKey(x => x.CityID)
                .OnDelete(DeleteBehavior.Restrict);

            // Hall - Cinema (One-to-Many)
            modelBuilder.Entity<Hall>()
                .HasOne(x => x.Cinema)
                .WithMany(x => x.Halls)
                .HasForeignKey(x => x.CinemaID)
                .OnDelete(DeleteBehavior.Restrict);

            // Seat - Hall (One-to-Many)
            modelBuilder.Entity<Seat>()
                .HasOne(x => x.Hall)
                .WithMany(x => x.Seats)
                .HasForeignKey(x => x.HallID)
                .OnDelete(DeleteBehavior.Restrict);

            // Session - Hall (One-to-Many)
            modelBuilder.Entity<Session>()
                .HasOne(x => x.Hall)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.HallID)
                .OnDelete(DeleteBehavior.Restrict);

            // Session - Cinema (One-to-Many)
            modelBuilder.Entity<Session>()
                .HasOne(x => x.Cinema)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.CinemaID)
                .OnDelete(DeleteBehavior.Restrict);

            // Pricing - Session (One-to-Many)
            modelBuilder.Entity<Pricing>()
                .HasOne(x => x.Session)
                .WithMany(x => x.Pricings)
                .HasForeignKey(x => x.SessionID)
                .OnDelete(DeleteBehavior.Restrict);

            // Pricing - Category (One-to-Many)
            modelBuilder.Entity<Pricing>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Pricings)
                .HasForeignKey(x => x.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket - Session (One-to-Many)
            modelBuilder.Entity<Domain.Entities.Ticket>()
                .HasOne(x => x.Session)
                .WithMany(x => x.Tickets)
                .HasForeignKey(x => x.SessionID)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket - Seat (One-to-Many)
            modelBuilder.Entity<Domain.Entities.Ticket>()
                .HasOne(x => x.Seat)
                .WithMany(x => x.Tickets)
                .HasForeignKey(x => x.SeatID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
