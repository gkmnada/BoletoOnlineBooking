using AutoMapper;
using Boleto.Contracts.Enums.Tickets;
using Boleto.Contracts.Events.PaymentEvents;
using Boleto.Contracts.Events.TicketEvents;
using Iyzipay.Model;
using Iyzipay.Request;
using MassTransit;
using MediatR;
using Newtonsoft.Json;
using Payment.API.Clients;
using Payment.API.Common.Base;
using Payment.API.Features.Payment.Commands;
using Payment.API.Models;
using Payment.API.Options;
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
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentService> _logger;
        private readonly OrderClient _orderClient;
        private readonly string _connectionString;
        private readonly string _userID;

        public PaymentService(IHttpContextAccessor httpContextAccessor, IPublishEndpoint publishEndpoint, IMapper mapper, IMediator mediator, ILogger<PaymentService> logger, OrderClient orderClient, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RedisDatabase") ?? "localhost";
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(_connectionString);
            _database = connection.GetDatabase();
            _httpContextAccessor = httpContextAccessor;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _mediator = mediator;
            _userID = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            _logger = logger;
            _orderClient = orderClient;
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
                    var ticket = JsonConvert.DeserializeObject<Ticket>(item!);
                    price += ticket!.Price;
                }

                var formattedPrice = price.ToString("F2", CultureInfo.InvariantCulture);

                Iyzipay.Options options = new Iyzipay.Options();
                options.ApiKey = PaymentOptions.ApiKey;
                options.SecretKey = PaymentOptions.SecretKey;
                options.BaseUrl = PaymentOptions.BaseURL;

                CreatePaymentRequest request = new CreatePaymentRequest();
                request.Locale = Locale.TR.ToString();
                request.ConversationId = "123456789";
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
                buyer.Id = "123456789";
                buyer.Name = "Gökmen";
                buyer.Surname = "Ada";
                buyer.GsmNumber = "+905350000000";
                buyer.Email = "gkmenada@hotmail.com";
                buyer.IdentityNumber = "74300864791";
                buyer.LastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                buyer.RegistrationDate = "2024-11-25 12:00:00";
                buyer.RegistrationAddress = "Çankaya";
                buyer.Ip = "85.34.78.112";
                buyer.City = "Ankara";
                buyer.Country = "Turkey";
                buyer.ZipCode = "06000";
                request.Buyer = buyer;

                Address shippingAddress = new Address();
                shippingAddress.ContactName = "Gökmen Ada";
                shippingAddress.City = "Ankara";
                shippingAddress.Country = "Turkey";
                shippingAddress.Description = "Çankaya";
                shippingAddress.ZipCode = "06000";
                request.ShippingAddress = shippingAddress;

                Address billingAddress = new Address();
                billingAddress.ContactName = "Gökmen Ada";
                billingAddress.City = "Ankara";
                billingAddress.Country = "Turkey";
                billingAddress.Description = "Çankaya";
                billingAddress.ZipCode = "06000";
                request.BillingAddress = billingAddress;

                List<BasketItem> basketItems = new List<BasketItem>();

                foreach (var item in values)
                {
                    BasketItem basketItem = new BasketItem();

                    var ticket = JsonConvert.DeserializeObject<Ticket>(item!);

                    basketItem.Id = ticket!.TicketID.ToString();
                    basketItem.Name = "Boleto Ticket";
                    basketItem.Category1 = "Movie";
                    basketItem.Category2 = "Online Booking";
                    basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                    basketItem.Price = ticket.Price.ToString("F2", CultureInfo.InvariantCulture);
                    basketItems.Add(basketItem);
                }

                request.BasketItems = basketItems;

                Iyzipay.Model.Payment payment = await Iyzipay.Model.Payment.Create(request, options);

                var tickets = new List<Ticket>();

                if (payment.Status == "success")
                {
                    // Update ticket status (Purchased) for ticket service
                    foreach (var item in values)
                    {
                        var ticket = JsonConvert.DeserializeObject<Ticket>(item!);
                        ticket!.Status = TicketStatus.Purchased.ToString();
                        ticket.Price = ticket.Price;

                        tickets.Add(ticket);
                    }

                    var success = tickets.Select(item => _publishEndpoint.Publish(_mapper.Map<TicketUpdated>(item)));
                    await Task.WhenAll(success);

                    // Create order for order service
                    var createOrderRequest = new CreateOrderRequest
                    {
                        MovieID = tickets.First().MovieID,
                        Status = TicketStatus.Purchased.ToString(),
                        TotalAmount = price,
                        CouponCode = tickets.Select(x => x.CouponCode).FirstOrDefault() ?? "",
                        DiscountAmount = tickets.Select(x => x.DiscountAmount).FirstOrDefault(),
                        UserID = _userID,
                        OrderDetails = tickets.Select(ticket => new CreateOrderDetailRequest
                        {
                            MovieID = ticket.MovieID,
                            CinemaID = ticket.CinemaID,
                            HallID = ticket.HallID,
                            SessionID = ticket.SessionID,
                            SeatID = ticket.SeatID,
                            Status = ticket.Status,
                            Price = ticket.Price
                        }).ToList()
                    };

                    var response = await _orderClient.CreateOrder(createOrderRequest);

                    // Create payment
                    var command = new CreatePaymentCommand
                    {
                        ConversationID = payment.ConversationId,
                        ProcessID = payment.PaymentId,
                        TransactionsID = payment.PaymentItems.Select(x => x.PaymentTransactionId).ToList(),
                        PaymentMethod = payment.CardAssociation,
                        PaymentType = Convert.ToInt16(payment.Installment),
                        CardType = payment.CardType,
                        BinNumber = payment.BinNumber,
                        LastFourDigits = payment.LastFourDigits,
                        PaymentAmount = Convert.ToDecimal(payment.Price),
                        OrderID = response.OrderID
                    };

                    await _mediator.Send(command);

                    //Clear the bookings for booking service 
                    var paymentCompleted = new PaymentCompleted
                    {
                        UserID = _userID,
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
                    var ticket = JsonConvert.DeserializeObject<Ticket>(item!);
                    ticket!.Status = TicketStatus.PaymentFailed.ToString();

                    tickets.Add(ticket);
                }

                var failed = tickets.Select(item => _publishEndpoint.Publish(_mapper.Map<TicketUpdated>(item)));
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
