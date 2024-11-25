using FluentValidation;
using Ticket.Application.Features.Seat.Commands;

namespace Ticket.Application.Features.Seat.Validators
{
    public class CreateSeatValidator : AbstractValidator<CreateSeatCommand>
    {
        public CreateSeatValidator()
        {
            RuleFor(x => x.Row).NotEmpty().WithMessage("Row is required");
            RuleFor(x => x.Number).NotEmpty().WithMessage("Number is required");
            RuleFor(x => x.HallID).NotEmpty().WithMessage("Hall ID is required");
        }
    }
}
