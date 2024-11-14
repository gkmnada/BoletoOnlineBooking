using Filter.API.Helpers;
using Filter.API.Models;

namespace Filter.API.Services
{
    public interface IFilterService
    {
        Task<List<Movie>> MovieFilterAsync(MovieFilter movieFilter);
    }
}
