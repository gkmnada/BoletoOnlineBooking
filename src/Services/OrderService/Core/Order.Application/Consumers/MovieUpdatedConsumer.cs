using Boleto.Contracts.Events.MovieEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Application.Interfaces.Services;

namespace Order.Application.Consumers
{
    public class MovieUpdatedConsumer : IConsumer<MovieUpdated>
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger<MovieUpdatedConsumer> _logger;

        public MovieUpdatedConsumer(IRedisCacheService redisCacheService, ILogger<MovieUpdatedConsumer> logger)
        {
            _redisCacheService = redisCacheService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MovieUpdated> context)
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
