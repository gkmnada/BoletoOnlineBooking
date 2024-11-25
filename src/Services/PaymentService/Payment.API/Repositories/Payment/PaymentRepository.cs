using MongoDB.Driver;
using Payment.API.Settings;

namespace Payment.API.Repositories.Payment
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMongoCollection<Entities.Payment> _paymentCollection;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(IDatabaseSettings databaseSettings, ILogger<PaymentRepository> logger)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _paymentCollection = database.GetCollection<Entities.Payment>(databaseSettings.PaymentCollectionName);
            _logger = logger;
        }

        public async Task CreatePaymentAsync(Entities.Payment payment)
        {
            try
            {
                await _paymentCollection.InsertOneAsync(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating payment");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
