namespace Discount.API.Repositories
{
    public interface ICouponRepository
    {
        Task<List<Entities.Coupon>> ListCouponAsync();
        Task CreateCouponAsync(Entities.Coupon coupon);
        Task UpdateCouponAsync(Entities.Coupon coupon);
        Task DeleteCouponAsync(Entities.Coupon coupon);
        Task<Entities.Coupon> GetCouponByIdAsync(string id);
        Task<Entities.Coupon> GetCouponByCodeAsync(string code);
    }
}
