using AutoMapper;
using Boleto.Contracts.Enums.MovieTicket;
using Boleto.Contracts.Events.BookingEvents;
using Boleto.Contracts.Events.TicketEvents;
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

        public async Task<BaseResponse> BookingCheckoutAsync()
        {
            try
            {
                var key = $"Bookings:{_userID}";

                var values = await _database.ListRangeAsync(key);

                if (values.Length == 0)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "No items found to checkout"
                    };
                }

                var checkouts = values.Select(item => JsonConvert.DeserializeObject<MovieTicket>(item!))
                    .Select(ticket => _mapper.Map<Checkout>(ticket!)).ToList();

                if (checkouts.Count > 0)
                {
                    var tasks = checkouts.Select(item => _publishEndpoint.Publish(_mapper.Map<BookingCheckout>(item)));
                    await Task.WhenAll(tasks);

                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = "Booking checkout is successfully completed"
                    };
                }

                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "No items found to checkout"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the booking checkout");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }

        public async Task<BaseResponse> CancelCheckoutAsync()
        {
            try
            {
                var key = $"Bookings:{_userID}";

                var values = await _database.ListRangeAsync(key);

                if (values.Length == 0)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "No items found to cancel the booking checkout"
                    };
                }

                var tickets = new List<MovieTicket>();

                foreach (var item in values)
                {
                    try
                    {
                        var ticket = JsonConvert.DeserializeObject<MovieTicket>(item!);
                        ticket!.status = MovieTicketStatus.Cancelled.ToString();

                        tickets.Add(ticket);
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, "An error occurred while deserializing the ticket object");
                        continue;
                    }
                }

                var tasks = tickets.Select(ticket => _publishEndpoint.Publish(_mapper.Map<MovieTicketUpdated>(ticket)));
                await Task.WhenAll(tasks);

                await _database.KeyDeleteAsync(key);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Booking checkout is successfully cancelled"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while canceling the booking checkout");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }

        public async Task<BaseResponse> ImplementCouponAsync(string couponCode)
        {
            try
            {
                var key = $"Bookings:{_userID}";

                if (string.IsNullOrWhiteSpace(couponCode))
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Coupon code is required"
                    };
                }

                var couponkey = $"ImplementedCoupons:{_userID}";
                var isCouponImplemented = await _database.SetContainsAsync(couponkey, couponCode);

                if (isCouponImplemented)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Coupon code is already implemented"
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

                var values = await _database.ListLengthAsync(key);

                if (values == 0)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "No items found to apply the coupon"
                    };
                }

                for (var index = 0; index < values; index++)
                {
                    var response = await _database.ListGetByIndexAsync(key, index);

                    var ticket = JsonConvert.DeserializeObject<MovieTicket>(response!);
                    ticket!.price = ticket.price - (ticket.price * discount.Amount / 100);

                    await _database.ListSetByIndexAsync(key, index, JsonConvert.SerializeObject(ticket));
                }

                await _database.SetAddAsync(couponkey, couponCode);

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
