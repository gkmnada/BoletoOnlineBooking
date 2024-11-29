using Catalog.Application.Features.MovieImage.Commands;
using Catalog.Application.Features.MovieImage.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovieImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("ListMovieImages/{id}")]
        public async Task<IActionResult> ListMovieImages(string id)
        {
            var query = new GetMovieImagesQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("GetMovieImage/{id}")]
        public async Task<IActionResult> GetMovieImageById(string id)
        {
            var query = new GetMovieImageByIdQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovieImage([FromForm] CreateMovieImageCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovieImage([FromForm] UpdateMovieImageCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieImage(string id)
        {
            var command = new DeleteMovieImageCommand(id);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
