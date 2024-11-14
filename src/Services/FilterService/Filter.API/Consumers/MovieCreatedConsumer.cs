using AutoMapper;
using Boleto.Contracts.Events.MovieEvents;
using Elastic.Clients.Elasticsearch;
using Filter.API.Models;
using MassTransit;

namespace Filter.API.Consumers
{
    public class MovieCreatedConsumer : IConsumer<MovieCreated>
    {
        private readonly IMapper _mapper;
        private readonly ElasticsearchClient _elasticClient;
        private readonly ILogger<MovieCreatedConsumer> _logger;
        private string _indexName;

        public MovieCreatedConsumer(IMapper mapper, ElasticsearchClient elasticsearchClient, ILogger<MovieCreatedConsumer> logger, IConfiguration configuration)
        {
            _mapper = mapper;
            _elasticClient = elasticsearchClient;
            _indexName = configuration["Elasticsearch:Index"] ?? "";
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MovieCreated> context)
        {
            try
            {
                var movie = _mapper.Map<Movie>(context.Message);
                movie.MovieID = context.Message.MovieID;

                await _elasticClient.IndexAsync(movie, x => x.Index(_indexName).Id(context.Message.MovieID));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the message");
                throw new Exception("An error occurred while processing the message", ex);
            }
        }
    }
}
