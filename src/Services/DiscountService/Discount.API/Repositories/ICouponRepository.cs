using Discount.API.Entities;
using Discount.API.Models;

namespace Discount.API.Repositories
{
    public interface ICouponRepository
    {
        Task CreateCouponAsync(CouponModel couponModel);
        Task<Coupon> GetCouponByCodeAsync(string code);
    }
}
