using AutoMapper;
using Discount.API.Features.Coupon.Queries;
using Discount.API.Features.Coupon.Results;
using Discount.API.Repositories;
using MediatR;

namespace Discount.API.Features.Coupon.Handlers.QueryHandlers
{
    public class GetCouponByCodeQueryHandler : IRequestHandler<GetCouponByCodeQuery, GetCouponByCodeQueryResult>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCouponByCodeQueryHandler> _logger;

        public GetCouponByCodeQueryHandler(ICouponRepository couponRepository, IMapper mapper, ILogger<GetCouponByCodeQueryHandler> logger)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetCouponByCodeQueryResult> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _couponRepository.GetCouponByCodeAsync(request.CouponCode);
                return _mapper.Map<GetCouponByCodeQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching coupon by code");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
