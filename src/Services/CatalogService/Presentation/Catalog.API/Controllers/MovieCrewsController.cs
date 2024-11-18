using Catalog.Application.Features.MovieCrew.Commands;
using Catalog.Application.Features.MovieCrew.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieCrewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovieCrewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("ListMovieCrews/{id}")]
        public async Task<IActionResult> ListMovieCrews(string id)
        {
            var query = new GetMovieCrewsQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("GetMovieCrew/{id}")]
        public async Task<IActionResult> GetMovieCrew(string id)
        {
            var query = new GetMovieCrewByIdQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovieCrew(CreateMovieCrewCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovieCrew(UpdateMovieCrewCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieCrew(string id)
        {
            var command = new DeleteMovieCrewCommand(id);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
