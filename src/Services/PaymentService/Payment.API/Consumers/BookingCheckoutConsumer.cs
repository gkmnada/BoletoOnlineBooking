using Boleto.Contracts.Events.BookingEvents;
using MassTransit;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace Payment.API.Consumers
{
    public class BookingCheckoutConsumer : IConsumer<BookingCheckout>
    {
        private readonly IDatabase _database;
        private readonly ILogger<BookingCheckoutConsumer> _logger;
        private readonly string _connectionString;

        public BookingCheckoutConsumer(ILogger<BookingCheckoutConsumer> logger, IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("RedisDatabase") ?? "localhost";
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(_connectionString);
            _database = connection.GetDatabase();
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BookingCheckout> context)
        {
            try
            {
                var key = $"BookingCheckouts:{context.Message.user_id}";

                if (await _database.KeyExistsAsync(key))
                {
                    await _database.KeyDeleteAsync(key);
                }

                var messages = JsonConvert.SerializeObject(context.Message);

                await _database.ListRightPushAsync(key, messages);
                await _database.KeyExpireAsync(key, TimeSpan.FromMinutes(5));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the message");
                throw new Exception("An error occurred while processing the message", ex);
            }
        }
    }
}
