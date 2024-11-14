using AutoMapper;
using Boleto.Contracts.Events.MovieEvents;
using MassTransit;
using MongoDB.Driver;
using Search.API.Models;
using Search.API.Settings;

namespace Search.API.Consumers
{
    public class MovieUpdatedConsumer : IConsumer<MovieUpdated>
    {
        private readonly IMongoCollection<Movie> _movieCollection;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieUpdatedConsumer> _logger;

        public MovieUpdatedConsumer(IMapper mapper, IDatabaseSettings databaseSettings, ILogger<MovieUpdatedConsumer> logger)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _movieCollection = database.GetCollection<Movie>(databaseSettings.MovieCollectionName);
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MovieUpdated> context)
        {
            try
            {
                var values = await _movieCollection.Find(x => x.MovieID == context.Message.MovieID).FirstOrDefaultAsync();

                if (values == null)
                {
                    throw new Exception("Movie not found");
                }

                _mapper.Map(context.Message, values);

                await _movieCollection.FindOneAndReplaceAsync(x => x.MovieID == context.Message.MovieID, values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the message");
                throw new Exception("An error occurred while processing the message", ex);
            }
        }
    }
}
