using Discount.API.Common.Base;
using MediatR;

namespace Discount.API.Features.Coupon.Commands
{
    public class DeleteCouponCommand : IRequest<BaseResponse>
    {
        public string CouponID { get; set; }

        public DeleteCouponCommand(string id)
        {
            CouponID = id;
        }
    }
}
