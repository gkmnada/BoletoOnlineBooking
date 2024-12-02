using Boleto.Contracts.Events.MovieEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Application.Interfaces.Services;

namespace Order.Application.Consumers
{
    public class MovieDeletedConsumer : IConsumer<MovieDeleted>
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger<MovieDeletedConsumer> _logger;

        public MovieDeletedConsumer(IRedisCacheService redisCacheService, ILogger<MovieDeletedConsumer> logger)
        {
            _redisCacheService = redisCacheService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MovieDeleted> context)
        {
            try
            {
                var key = $"Movies:{context.Message.MovieID}";
                await _redisCacheService.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the message");
                throw new Exception("An error occurred while processing the message", ex);
            }
        }
    }
}
