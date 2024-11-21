using FluentValidation;
using Ticket.Application.Features.Hall.Commands;

namespace Ticket.Application.Features.Hall.Validators
{
    public class CreateHallValidator : AbstractValidator<CreateHallCommand>
    {
        public CreateHallValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Name is required")
                .MinimumLength(2).WithMessage("Name must not be less than 2 characters")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
            RuleFor(x => x.capacity).NotEmpty().WithMessage("Capacity is required")
                .GreaterThan(0).WithMessage("Capacity must be greater than 0");
            RuleFor(x => x.cinema_id).NotEmpty().WithMessage("Cinema ID is required");
        }
    }
}
