using Search.API.Models;

namespace Search.API.Services
{
    public interface ISearchService
    {
        Task<List<Movie>> SearchAsync(string keyword);
    }
}
