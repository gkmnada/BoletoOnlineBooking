using Microsoft.EntityFrameworkCore;

namespace Order.Persistence.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Order> Orders { get; set; }
        public DbSet<Domain.Entities.OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Order - OrderDetail (One-to-Many)
            modelBuilder.Entity<Domain.Entities.OrderDetail>()
                .HasOne(x => x.Order)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            // Set precision for decimal columns
            modelBuilder.Entity<Domain.Entities.Order>(entity =>
            {
                entity.Property(x => x.TotalAmount).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Domain.Entities.OrderDetail>(entity =>
            {
                entity.Property(x => x.Price).HasPrecision(18, 2);
            });
        }
    }
}
