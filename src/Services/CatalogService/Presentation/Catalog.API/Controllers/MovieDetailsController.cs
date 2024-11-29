using Catalog.Application.Features.MovieDetail.Commands;
using Catalog.Application.Features.MovieDetail.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovieDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailByMovieId(string id)
        {
            var query = new GetMovieDetailByMovieIdQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("GetMovieDetail/{id}")]
        public async Task<IActionResult> GetMovieDetailById(string id)
        {
            var query = new GetMovieDetailByIdQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovieDetail([FromForm] CreateMovieDetailCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovieDetail([FromForm] UpdateMovieDetailCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieDetail(string id)
        {
            var command = new DeleteMovieDetailCommand(id);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
