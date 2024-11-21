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

        public DbSet<Category> categories { get; set; }
        public DbSet<Cinema> cinemas { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<Hall> halls { get; set; }
        public DbSet<MovieTicket> movie_tickets { get; set; }
        public DbSet<Pricing> pricings { get; set; }
        public DbSet<Seat> seats { get; set; }
        public DbSet<Session> sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cinema - City (One-to-Many)
            modelBuilder.Entity<Cinema>()
                .HasOne(x => x.city)
                .WithMany(x => x.cinemas)
                .HasForeignKey(x => x.city_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Hall - Cinema (One-to-Many)
            modelBuilder.Entity<Hall>()
                .HasOne(x => x.cinema)
                .WithMany(x => x.halls)
                .HasForeignKey(x => x.cinema_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Seat - Hall (One-to-Many)
            modelBuilder.Entity<Seat>()
                .HasOne(x => x.hall)
                .WithMany(x => x.seats)
                .HasForeignKey(x => x.hall_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Session - Hall (One-to-Many)
            modelBuilder.Entity<Session>()
                .HasOne(x => x.hall)
                .WithMany(x => x.sessions)
                .HasForeignKey(x => x.hall_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Session - Cinema (One-to-Many)
            modelBuilder.Entity<Session>()
                .HasOne(x => x.cinema)
                .WithMany(x => x.sessions)
                .HasForeignKey(x => x.cinema_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Pricing - Session (One-to-Many)
            modelBuilder.Entity<Pricing>()
                .HasOne(x => x.session)
                .WithMany(x => x.pricings)
                .HasForeignKey(x => x.session_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Pricing - Category (One-to-Many)
            modelBuilder.Entity<Pricing>()
                .HasOne(x => x.category)
                .WithMany(x => x.pricings)
                .HasForeignKey(x => x.category_id)
                .OnDelete(DeleteBehavior.Restrict);

            // MovieTicket - Session (One-to-Many)
            modelBuilder.Entity<MovieTicket>()
                .HasOne(x => x.session)
                .WithMany(x => x.movie_tickets)
                .HasForeignKey(x => x.session_id)
                .OnDelete(DeleteBehavior.Restrict);

            // MovieTicket - Seat (One-to-Many)
            modelBuilder.Entity<MovieTicket>()
                .HasOne(x => x.seat)
                .WithMany(x => x.movie_tickets)
                .HasForeignKey(x => x.seat_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
