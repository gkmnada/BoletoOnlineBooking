using Boleto.Contracts.Events.MovieEvents;
using Elastic.Clients.Elasticsearch;
using Filter.API.Models;
using MassTransit;

namespace Filter.API.Consumers
{
    public class MovieDeletedConsumer : IConsumer<MovieDeleted>
    {
        private readonly ElasticsearchClient _elasticClient;
        private readonly ILogger<MovieDeletedConsumer> _logger;
        private string _indexName;

        public MovieDeletedConsumer(ElasticsearchClient elasticClient, ILogger<MovieDeletedConsumer> logger, IConfiguration configuration)
        {
            _elasticClient = elasticClient;
            _indexName = configuration["Elasticsearch:Index"] ?? "";
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MovieDeleted> context)
        {
            try
            {
                var response = await _elasticClient.DeleteAsync<Movie>(context.Message.MovieID, x => x.Index(_indexName));

                if (!response.IsValidResponse)
                {
                    throw new Exception("An error occurred while deleting the movie");
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
