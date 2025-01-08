using Catalog.Application.Features.Movie.Commands;
using Catalog.Application.Features.Movie.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MoviesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ListMovies()
        {
            var query = new GetMoviesQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(string id)
        {
            var query = new GetMovieByIdQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromForm] CreateMovieCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovie([FromForm] UpdateMovieCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(string id)
        {
            var command = new DeleteMovieCommand(id);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
