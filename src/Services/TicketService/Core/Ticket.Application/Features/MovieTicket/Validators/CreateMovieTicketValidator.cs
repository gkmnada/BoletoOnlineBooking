using FluentValidation;
using Ticket.Application.Features.MovieTicket.Commands;

namespace Ticket.Application.Features.MovieTicket.Validators
{
    public class CreateMovieTicketValidator : AbstractValidator<CreateMovieTicketCommand>
    {
        public CreateMovieTicketValidator()
        {
            RuleFor(x=> x.session_id).NotEmpty().WithMessage("Session ID is required");
            RuleFor(x => x.seat_id).NotEmpty().WithMessage("Seat ID is required");
            RuleFor(x => x.status).NotEmpty().WithMessage("Status is required");
            RuleFor(x => x.price).NotEmpty().WithMessage("Price is required");
        }
    }
}
