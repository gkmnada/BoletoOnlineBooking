using Boleto.Contracts.Events.PaymentEvents;
using MassTransit;
using StackExchange.Redis;

namespace Booking.API.Consumers
{
    public class PaymentCompletedConsumer : IConsumer<PaymentCompleted>
    {
        private readonly IDatabase _database;
        private readonly ILogger<PaymentCompletedConsumer> _logger;
        private readonly string _connectionString;

        public PaymentCompletedConsumer(ILogger<PaymentCompletedConsumer> logger, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RedisDatabase") ?? "localhost";
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(_connectionString);
            _database = connection.GetDatabase();
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentCompleted> context)
        {
            try
            {
                var key = $"Bookings:{context.Message.user_id}";
                await _database.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the message");
                throw new Exception("An error occurred while processing the message", ex);
            }
        }
    }
}
