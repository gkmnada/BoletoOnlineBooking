using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticket.Application.Features.MovieTicket.Commands;

namespace Ticket.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieTicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovieTicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovieTicket(CreateMovieTicketCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
