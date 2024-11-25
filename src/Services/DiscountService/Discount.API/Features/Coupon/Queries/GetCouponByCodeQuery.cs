using Discount.API.Features.Coupon.Results;
using MediatR;

namespace Discount.API.Features.Coupon.Queries
{
    public class GetCouponByCodeQuery : IRequest<GetCouponByCodeQueryResult>
    {
        public string CouponCode { get; set; }

        public GetCouponByCodeQuery(string couponCode)
        {
            CouponCode = couponCode;
        }
    }
}
