using AutoMapper;
using Discount.API.Features.Coupon.Queries;
using Discount.API.Features.Coupon.Results;
using Discount.API.Repositories;
using MediatR;

namespace Discount.API.Features.Coupon.Handlers.QueryHandlers
{
    public class GetCouponByIdQueryHandler : IRequestHandler<GetCouponByIdQuery, GetCouponByIdQueryResult>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCouponByIdQueryHandler> _logger;

        public GetCouponByIdQueryHandler(ICouponRepository couponRepository, IMapper mapper, ILogger<GetCouponByIdQueryHandler> logger)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetCouponByIdQueryResult> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _couponRepository.GetCouponByIdAsync(request.CouponID);
                return _mapper.Map<GetCouponByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching coupon by ID");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
