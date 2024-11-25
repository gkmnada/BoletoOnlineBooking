using Discount.API.Common.Base;
using MediatR;

namespace Discount.API.Features.Coupon.Commands
{
    public class UpdateCouponCommand : IRequest<BaseResponse>
    {
        public string CouponID { get; set; }
        public string CouponCode { get; set; }
        public int Amount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
