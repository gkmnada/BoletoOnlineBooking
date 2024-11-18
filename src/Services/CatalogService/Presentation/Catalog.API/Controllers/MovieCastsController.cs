using Catalog.Application.Features.MovieCast.Commands;
using Catalog.Application.Features.MovieCast.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieCastsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovieCastsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("ListMovieCasts/{id}")]
        public async Task<IActionResult> ListMovieCasts(string id)
        {
            var query = new GetMovieCastsQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("GetMovieCast/{id}")]
        public async Task<IActionResult> GetMovieCast(string id)
        {
            var query = new GetMovieCastByIdQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovieCast(CreateMovieCastCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovieCast(UpdateMovieCastCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieCast(string id)
        {
            var command = new DeleteMovieCastCommand(id);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
