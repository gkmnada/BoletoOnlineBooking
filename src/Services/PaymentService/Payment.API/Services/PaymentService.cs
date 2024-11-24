using AutoMapper;
using Boleto.Contracts.Enums.MovieTicket;
using Boleto.Contracts.Events.PaymentEvents;
using Boleto.Contracts.Events.TicketEvents;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using MassTransit;
using Newtonsoft.Json;
using Payment.API.Common.Base;
using Payment.API.Models;
using Payment.API.Settings;
using StackExchange.Redis;
using System.Globalization;
using System.Security.Claims;

namespace Payment.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDatabase _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;
        private readonly string _connectionString;
        private readonly string _userID;

        public PaymentService(ILogger<PaymentService> logger, IHttpContextAccessor httpContextAccessor, IPublishEndpoint publishEndpoint, IMapper mapper, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RedisDatabase") ?? "localhost";
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(_connectionString);
            _database = connection.GetDatabase();
            _httpContextAccessor = httpContextAccessor;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _userID = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            _logger = logger;
        }

        public async Task<BaseResponse> ProcessPaymentAsync(PaymentForm paymentForm)
        {
            try
            {
                var key = $"BookingCheckouts:{_userID}";

                var values = await _database.ListRangeAsync(key);

                if (values.Length == 0)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "No items found to process payment"
                    };
                }

                decimal price = 0;

                foreach (var item in values)
                {
                    var ticket = JsonConvert.DeserializeObject<MovieTicket>(item!);
                    price += ticket!.price;
                }

                var formattedPrice = price.ToString("F2", CultureInfo.InvariantCulture);

                Options options = new Options();
                options.ApiKey = MerchantSettings.ApiKey;
                options.SecretKey = MerchantSettings.SecretKey;
                options.BaseUrl = MerchantSettings.BaseURL;

                CreatePaymentRequest request = new CreatePaymentRequest();
                request.Locale = Locale.TR.ToString();
                request.ConversationId = Guid.NewGuid().ToString("D");
                request.Price = formattedPrice;
                request.PaidPrice = formattedPrice;
                request.Currency = Currency.TRY.ToString();
                request.Installment = 1;
                request.BasketId = "B170899";
                request.PaymentChannel = PaymentChannel.WEB.ToString();
                request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

                PaymentCard paymentCard = new PaymentCard();
                paymentCard.CardHolderName = paymentForm.CardHolderName;
                paymentCard.CardNumber = paymentForm.CardNumber;
                paymentCard.ExpireMonth = paymentForm.ExpireMonth;
                paymentCard.ExpireYear = paymentForm.ExpireYear;
                paymentCard.Cvc = paymentForm.CVV;
                paymentCard.RegisterCard = 0;
                request.PaymentCard = paymentCard;

                Buyer buyer = new Buyer();
                buyer.Id = "BY789";
                buyer.Name = "John";
                buyer.Surname = "Doe";
                buyer.GsmNumber = "+905350000000";
                buyer.Email = "email@email.com";
                buyer.IdentityNumber = "74300864791";
                buyer.LastLoginDate = "2015-10-05 12:43:35";
                buyer.RegistrationDate = "2013-04-21 15:12:09";
                buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
                buyer.Ip = "85.34.78.112";
                buyer.City = "Istanbul";
                buyer.Country = "Turkey";
                buyer.ZipCode = "34732";
                request.Buyer = buyer;

                Address shippingAddress = new Address();
                shippingAddress.ContactName = "Jane Doe";
                shippingAddress.City = "Istanbul";
                shippingAddress.Country = "Turkey";
                shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
                shippingAddress.ZipCode = "34742";
                request.ShippingAddress = shippingAddress;

                Address billingAddress = new Address();
                billingAddress.ContactName = "Jane Doe";
                billingAddress.City = "Istanbul";
                billingAddress.Country = "Turkey";
                billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
                billingAddress.ZipCode = "34742";
                request.BillingAddress = billingAddress;

                List<BasketItem> basketItems = new List<BasketItem>();

                foreach (var item in values)
                {
                    BasketItem basketItem = new BasketItem();

                    var ticket = JsonConvert.DeserializeObject<MovieTicket>(item!);

                    basketItem.Id = ticket!.ticket_id.ToString();
                    basketItem.Name = ticket.movie_id;
                    basketItem.Category1 = "Online Booking";
                    basketItem.Category2 = "Movie";
                    basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                    basketItem.Price = ticket.price.ToString("F2", CultureInfo.InvariantCulture);
                    basketItems.Add(basketItem);
                }

                request.BasketItems = basketItems;

                Iyzipay.Model.Payment payment = await Iyzipay.Model.Payment.Create(request, options);

                var tickets = new List<MovieTicket>();

                if (payment.Status == "success")
                {
                    // Update ticket status (Purchased) for ticket service
                    foreach (var item in values)
                    {
                        var ticket = JsonConvert.DeserializeObject<MovieTicket>(item!);
                        ticket!.status = MovieTicketStatus.Purchased.ToString();
                        ticket.price = ticket.price;

                        tickets.Add(ticket);
                    }

                    var success = tickets.Select(item => _publishEndpoint.Publish(_mapper.Map<MovieTicketUpdated>(item)));
                    await Task.WhenAll(success);

                    //Clear the bookings for booking service 
                    var paymentCompleted = new PaymentCompleted
                    {
                        user_id = _userID,
                    };
                    await _publishEndpoint.Publish(paymentCompleted);

                    // Clear the booking checkouts
                    await _database.KeyDeleteAsync(key);

                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = "Payment is successful"
                    };
                }

                // Update ticket status (PaymentFailed) for ticket service
                foreach (var item in values)
                {
                    var ticket = JsonConvert.DeserializeObject<MovieTicket>(item!);
                    ticket!.status = MovieTicketStatus.PaymentFailed.ToString();

                    tickets.Add(ticket);
                }

                var failed = tickets.Select(item => _publishEndpoint.Publish(_mapper.Map<MovieTicketUpdated>(item)));
                await Task.WhenAll(failed);

                await _database.KeyDeleteAsync(key);

                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Payment is not successful"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing payment");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
