using Boleto.Contracts.Events.MovieEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Application.Interfaces.Services;

namespace Order.Application.Consumers
{
    public class MovieCreatedConsumer : IConsumer<MovieCreated>
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger<MovieCreatedConsumer> _logger;

        public MovieCreatedConsumer(IRedisCacheService redisCacheService, ILogger<MovieCreatedConsumer> logger)
        {
            _redisCacheService = redisCacheService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MovieCreated> context)
        {
            try
            {
                var key = $"Movies:{context.Message.MovieID}";
                await _redisCacheService.SetAsync(key, context.Message.MovieName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the message");
                throw new Exception("An error occurred while processing the message", ex);
            }
        }
    }
}
