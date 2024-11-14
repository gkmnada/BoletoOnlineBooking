using Microsoft.AspNetCore.Mvc;
using Search.API.Services;

namespace Search.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> SearchAsync(string keyword)
        {
            var response = await _searchService.SearchAsync(keyword);
            return Ok(response);
        }
    }
}
