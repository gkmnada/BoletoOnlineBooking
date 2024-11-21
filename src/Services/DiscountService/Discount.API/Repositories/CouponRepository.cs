using AutoMapper;
using Discount.API.Context;
using Discount.API.Entities;
using Discount.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.API.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public CouponRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateCouponAsync(CouponModel couponModel)
        {
            var entity = _mapper.Map<Coupon>(couponModel);
            _context.Coupons.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Coupon> GetCouponByCodeAsync(string code)
        {
            var values = await _context.Coupons.FirstOrDefaultAsync(x => x.CouponCode == code);
            return values;
        }
    }
}
