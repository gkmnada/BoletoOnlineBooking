using Booking.API.Common.Base;

namespace Booking.API.Services
{
    public interface IBookingService
    {
        Task<BaseResponse> ImplementCouponAsync(string couponCode);
        Task<BaseResponse> BookingCheckoutAsync();
        Task<BaseResponse> CancelCheckoutAsync();
    }
}
