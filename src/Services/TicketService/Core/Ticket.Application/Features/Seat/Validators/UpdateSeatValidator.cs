using FluentValidation;
using Ticket.Application.Features.Seat.Commands;

namespace Ticket.Application.Features.Seat.Validators
{
    public class UpdateSeatValidator : AbstractValidator<UpdateSeatCommand>
    {
        public UpdateSeatValidator()
        {
            RuleFor(x => x.SeatID).NotEmpty().WithMessage("Seat ID is required");
            RuleFor(x => x.Row).NotEmpty().WithMessage("Row is required");
            RuleFor(x => x.Number).NotEmpty().WithMessage("Number is required");
            RuleFor(x => x.HallID).NotEmpty().WithMessage("Hall ID is required");
        }
    }
}
