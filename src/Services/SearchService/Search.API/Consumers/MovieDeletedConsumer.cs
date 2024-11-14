using Boleto.Contracts.Events.MovieEvents;
using MassTransit;
using MongoDB.Driver;
using Search.API.Models;
using Search.API.Settings;

namespace Search.API.Consumers
{
    public class MovieDeletedConsumer : IConsumer<MovieDeleted>
    {
        private readonly IMongoCollection<Movie> _movieCollection;
        private readonly ILogger<MovieDeletedConsumer> _logger;

        public MovieDeletedConsumer(ILogger<MovieDeletedConsumer> logger, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _movieCollection = database.GetCollection<Movie>(databaseSettings.MovieCollectionName);
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MovieDeleted> context)
        {
            try
            {
                await _movieCollection.FindOneAndDeleteAsync(x => x.MovieID == context.Message.MovieID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the message");
                throw new Exception("An error occurred while processing the message", ex);
            }
        }
    }
}
