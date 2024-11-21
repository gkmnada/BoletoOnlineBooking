using Discount.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discount.API.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }
    }
}
