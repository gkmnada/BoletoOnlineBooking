using AutoMapper;
using Discount.API.Features.Coupon.Queries;
using Discount.API.Features.Coupon.Results;
using Discount.API.Repositories;
using MediatR;

namespace Discount.API.Features.Coupon.Handlers.QueryHandlers
{
    public class GetCouponsQueryHandler : IRequestHandler<GetCouponsQuery, List<GetCouponsQueryResult>>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCouponsQueryHandler> _logger;

        public GetCouponsQueryHandler(ICouponRepository couponRepository, IMapper mapper, ILogger<GetCouponsQueryHandler> logger)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetCouponsQueryResult>> Handle(GetCouponsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _couponRepository.ListCouponAsync();
                return _mapper.Map<List<GetCouponsQueryResult>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching coupons");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
