using FluentValidation;
using Ticket.Application.Features.Hall.Commands;

namespace Ticket.Application.Features.Hall.Validators
{
    public class UpdateHallValidator : AbstractValidator<UpdateHallCommand>
    {
        public UpdateHallValidator()
        {
            RuleFor(x => x.HallID).NotEmpty().WithMessage("Hall ID is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .MinimumLength(2).WithMessage("Name must not be less than 2 characters")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
            RuleFor(x => x.Capacity).NotEmpty().WithMessage("Capacity is required")
                .GreaterThan(0).WithMessage("Capacity must be greater than 0");
            RuleFor(x => x.CinemaID).NotEmpty().WithMessage("Cinema ID is required");
        }
    }
}
