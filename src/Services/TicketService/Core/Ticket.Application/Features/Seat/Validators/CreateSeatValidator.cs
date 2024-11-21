using FluentValidation;
using Ticket.Application.Features.Seat.Commands;

namespace Ticket.Application.Features.Seat.Validators
{
    public class CreateSeatValidator : AbstractValidator<CreateSeatCommand>
    {
        public CreateSeatValidator()
        {
            RuleFor(x => x.row).NotEmpty().WithMessage("Row is required");
            RuleFor(x => x.number).NotEmpty().WithMessage("Number is required");
            RuleFor(x => x.hall_id).NotEmpty().WithMessage("Hall ID is required");
        }
    }
}
