using Catalog.Application.Features.Category.Commands;
using Catalog.Application.Features.Category.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var query = new GetCategoriesQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var query = new GetCategoryByIdQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var command = new DeleteCategoryCommand(id);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
