using AutoMapper;
using Boleto.Contracts.Events.MovieEvents;
using Elastic.Clients.Elasticsearch;
using Filter.API.Models;
using MassTransit;

namespace Filter.API.Consumers
{
    public class MovieUpdatedConsumer : IConsumer<MovieUpdated>
    {
        private readonly IMapper _mapper;
        private readonly ElasticsearchClient _elasticClient;
        private readonly ILogger<MovieUpdatedConsumer> _logger;
        private string _indexName;

        public MovieUpdatedConsumer(IMapper mapper, ElasticsearchClient elasticClient, ILogger<MovieUpdatedConsumer> logger, IConfiguration configuration)
        {
            _mapper = mapper;
            _elasticClient = elasticClient;
            _indexName = configuration["Elasticsearch:Index"] ?? "";
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MovieUpdated> context)
        {
            try
            {
                var movie = _mapper.Map<Movie>(context.Message);

                var response = await _elasticClient.UpdateAsync<Movie, object>
                    (context.Message.MovieID, x => x.Index(_indexName).Doc(movie).DocAsUpsert(true));

                if (!response.IsValidResponse)
                {
                    throw new Exception("An error occurred while updating the movie");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the message");
                throw new Exception("An error occurred while processing the message", ex);
            }
        }
    }
}
