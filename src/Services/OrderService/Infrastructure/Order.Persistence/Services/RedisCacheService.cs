using Order.Application.Interfaces.Services;
using StackExchange.Redis;

namespace Order.Persistence.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<string> GetAsync(string key)
        {
            var database = _connectionMultiplexer.GetDatabase();
            return await database.StringGetAsync(key);
        }

        public async Task RemoveAsync(string key)
        {
            var database = _connectionMultiplexer.GetDatabase();
            await database.KeyDeleteAsync(key);
        }

        public async Task SetAsync(string key, string value)
        {
            var database = _connectionMultiplexer.GetDatabase();
            await database.StringSetAsync(key, value);
        }
    }
}
