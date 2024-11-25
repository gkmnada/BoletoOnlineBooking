using FluentValidation;
using Ticket.Application.Features.Ticket.Commands;

namespace Ticket.Application.Features.Ticket.Validators
{
    public class CreateTicketValidator : AbstractValidator<CreateTicketCommand>
    {
        public CreateTicketValidator()
        {
            RuleFor(x => x.SessionID).NotEmpty().WithMessage("Session ID is required");
            RuleFor(x => x.SeatID).NotEmpty().WithMessage("Seat ID is required");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
        }
    }
}
