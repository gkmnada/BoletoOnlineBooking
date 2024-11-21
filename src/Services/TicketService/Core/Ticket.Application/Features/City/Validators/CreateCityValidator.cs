using FluentValidation;
using Ticket.Application.Features.City.Commands;

namespace Ticket.Application.Features.City.Validators
{
    public class CreateCityValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Name is required")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters")
                .MaximumLength(15).WithMessage("Name must not exceed 15 characters");
        }
    }
}
