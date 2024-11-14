using Filter.API.Helpers;
using Filter.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly IFilterService _filterService;

        public FilterController(IFilterService filterService)
        {
            _filterService = filterService;
        }

        [HttpPost]
        public async Task<IActionResult> FilterMoviesAsync(MovieFilter movieFilter)
        {
            var response = await _filterService.MovieFilterAsync(movieFilter);
            return Ok(response);
        }
    }
}
