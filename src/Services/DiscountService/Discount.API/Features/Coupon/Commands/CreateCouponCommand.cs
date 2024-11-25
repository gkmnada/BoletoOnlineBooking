using Discount.API.Common.Base;
using MediatR;

namespace Discount.API.Features.Coupon.Commands
{
    public class CreateCouponCommand : IRequest<BaseResponse>
    {
        public string CouponCode { get; set; }
        public int Amount { get; set; }
    }
}
