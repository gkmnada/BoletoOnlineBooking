using FluentValidation;
using Ticket.Application.Features.Cinema.Commands;

namespace Ticket.Application.Features.Cinema.Validators
{
    public class CreateCinemaValidator : AbstractValidator<CreateCinemaCommand>
    {
        public CreateCinemaValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must not be less than 3 characters")
                .MaximumLength(50).WithMessage("Name must not be more than 50 characters");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required")
                .MinimumLength(3).WithMessage("Address must not be less than 3 characters")
                .MaximumLength(100).WithMessage("Address must not be more than 100 characters");
            RuleFor(x => x.CityID).NotEmpty().WithMessage("City ID is required");
        }
    }
}
