using AutoMapper;
using Booking.API.Clients;
using Booking.API.Common.Base;
using Booking.API.Models;
using MassTransit;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Security.Claims;

namespace Booking.API.Services
{
    public class BookingService : IBookingService
    {
        private readonly IDatabase _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BookingService> _logger;
        private readonly DiscountClient _discountClient;
        private readonly string _userID;
        private readonly string _connectionString;

        public BookingService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IPublishEndpoint publishEndpoint, ILogger<BookingService> logger, DiscountClient discountClient, IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("RedisDatabase") ?? "localhost";
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(_connectionString);
            _database = connection.GetDatabase();
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _discountClient = discountClient;
            _userID = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        public async Task<BaseResponse> ImplementCouponAsync(string couponCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(couponCode))
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Coupon code is required"
                    };
                }

                var discount = _discountClient.GetDiscount(couponCode);

                if (discount == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Coupon code is invalid"
                    };
                }

                var listLength = await _database.ListLengthAsync(_userID);

                if (listLength == 0)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "No items found to apply the coupon"
                    };
                }

                for (var index = 0; index < listLength; index++)
                {
                    var response = await _database.ListGetByIndexAsync(_userID, index);

                    if (response.IsNullOrEmpty) continue;

                    var ticket = JsonConvert.DeserializeObject<MovieTicket>(response);
                    ticket.price = ticket.price - (ticket.price * discount.Amount / 100);

                    await _database.ListSetByIndexAsync(_userID, index, JsonConvert.SerializeObject(ticket));
                }

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Coupon code is successfully implemented"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while implementing the coupon code");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
