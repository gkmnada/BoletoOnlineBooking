using Discount.API.Features.Coupon.Results;
using MediatR;

namespace Discount.API.Features.Coupon.Queries
{
    public class GetCouponByIdQuery : IRequest<GetCouponByIdQueryResult>
    {
        public string CouponID { get; set; }

        public GetCouponByIdQuery(string id)
        {
            CouponID = id;
        }
    }
}
