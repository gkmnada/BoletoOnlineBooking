namespace Order.Application.Interfaces.Services
{
    public interface IRedisCacheService
    {
        Task<string> GetAsync(string key);
        Task SetAsync(string key, string value);
        Task RemoveAsync(string key);
    }
}
