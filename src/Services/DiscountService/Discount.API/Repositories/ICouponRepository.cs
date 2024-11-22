using Discount.API.Common.Base;
using Discount.API.Dtos.Coupon;

namespace Discount.API.Repositories
{
    public interface ICouponRepository
    {
        Task<List<ListCouponDto>> ListCouponAsync();
        Task<BaseResponse> CreateCouponAsync(CreateCouponDto createCouponDto);
        Task<BaseResponse> UpdateCouponAsync(UpdateCouponDto updateCouponDto);
        Task<BaseResponse> DeleteCouponAsync(string id);
        Task<CouponDto> GetCouponByIdAsync(string id);
        Task<CouponDto> GetCouponByCodeAsync(string code);
        
    }
}
