using Discount.API.Features.Coupon.Results;
using MediatR;

namespace Discount.API.Features.Coupon.Queries
{
    public class GetCouponsQuery : IRequest<List<GetCouponsQueryResult>>
    {
    }
}
