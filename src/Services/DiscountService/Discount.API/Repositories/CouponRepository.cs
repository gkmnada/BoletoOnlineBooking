using Discount.API.Context;
using Discount.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discount.API.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<CouponRepository> _logger;

        public CouponRepository(ApplicationContext context, ILogger<CouponRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateCouponAsync(Coupon coupon)
        {
            await _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCouponAsync(Coupon coupon)
        {
            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task<Coupon> GetCouponByCodeAsync(string code)
        {
            return await _context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == code);
        }

        public async Task<Coupon> GetCouponByIdAsync(string id)
        {
            return await _context.Coupons.FindAsync(id);
        }

        public async Task<List<Coupon>> ListCouponAsync()
        {
            return await _context.Coupons.ToListAsync();
        }

        public async Task UpdateCouponAsync(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
            await _context.SaveChangesAsync();
        }
    }
}
