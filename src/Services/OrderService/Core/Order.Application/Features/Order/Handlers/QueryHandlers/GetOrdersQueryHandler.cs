using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Order.Application.Features.Order.Queries;
using Order.Application.Features.Order.Results;
using Order.Application.Interfaces.Repositories;
using Order.Application.Interfaces.Services;
using System.Security.Claims;

namespace Order.Application.Features.Order.Handlers.QueryHandlers
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<GetOrdersQueryResult>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetOrdersQueryHandler> _logger;
        private readonly string _userID;

        public GetOrdersQueryHandler(IOrderRepository orderRepository, IRedisCacheService redisCacheService, IHttpContextAccessor httpContextAccessor, ILogger<GetOrdersQueryHandler> logger)
        {
            _orderRepository = orderRepository;
            _redisCacheService = redisCacheService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _userID = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        public async Task<List<GetOrdersQueryResult>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _orderRepository.ListByFilterAsync(x => x.UserID == _userID, cancellationToken);

                var movies = values.Select(x => x.MovieID).Distinct().ToList();

                var keys = movies.Select(id => $"Movies:{id}").ToArray();

                foreach (var key in keys)
                {
                    await _redisCacheService.GetAsync(key);
                }
                var response = await Task.WhenAll(keys.Select(async key => await _redisCacheService.GetAsync(key)));

                var dictionary = new Dictionary<string, string>();
                for (int i = 0; i < movies.Count; i++)
                {
                    dictionary[movies[i]] = response[i];
                }

                var orders = values.Select(x => new GetOrdersQueryResult
                {
                    OrderID = x.OrderID,
                    MovieName = dictionary[x.MovieID] ?? "Bilinmeyen Film",
                    Status = x.Status,
                    TotalAmount = x.TotalAmount,
                    CouponCode = x.CouponCode,
                    DiscountAmount = x.DiscountAmount,
                }).ToList();

                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the orders");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
