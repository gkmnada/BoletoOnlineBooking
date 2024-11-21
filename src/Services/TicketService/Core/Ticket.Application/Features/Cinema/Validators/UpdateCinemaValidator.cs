using FluentValidation;
using Ticket.Application.Features.Cinema.Commands;

namespace Ticket.Application.Features.Cinema.Validators
{
    public class UpdateCinemaValidator : AbstractValidator<UpdateCinemaCommand>
    {
        public UpdateCinemaValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Cinema ID is required");
            RuleFor(x => x.name).NotEmpty().WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must not be less than 3 characters")
                .MaximumLength(50).WithMessage("Name must not be more than 50 characters");
            RuleFor(x => x.address).NotEmpty().WithMessage("Address is required")
                .MinimumLength(3).WithMessage("Address must not be less than 3 characters")
                .MaximumLength(100).WithMessage("Address must not be more than 100 characters");
            RuleFor(x => x.city_id).NotEmpty().WithMessage("City ID is required");
        }
    }
}
