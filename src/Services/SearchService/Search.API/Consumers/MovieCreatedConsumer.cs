using AutoMapper;
using Boleto.Contracts.Events.MovieEvents;
using MassTransit;
using MongoDB.Driver;
using Search.API.Models;
using Search.API.Settings;

namespace Search.API.Consumers
{
    public class MovieCreatedConsumer : IConsumer<MovieCreated>
    {
        private readonly IMongoCollection<Movie> _movieCollection;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieCreatedConsumer> _logger;

        public MovieCreatedConsumer(IMapper mapper, IDatabaseSettings databaseSettings, ILogger<MovieCreatedConsumer> logger)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _movieCollection = database.GetCollection<Movie>(databaseSettings.MovieCollectionName);
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MovieCreated> context)
        {
            try
            {
                var movie = _mapper.Map<Movie>(context.Message);
                await _movieCollection.InsertOneAsync(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the message");
                throw new Exception("An error occurred while processing the message", ex);
            }
        }
    }
}
