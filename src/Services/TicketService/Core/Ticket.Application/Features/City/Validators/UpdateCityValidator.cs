using FluentValidation;
using Ticket.Application.Features.City.Commands;

namespace Ticket.Application.Features.City.Validators
{
    public class UpdateCityValidator : AbstractValidator<UpdateCityCommand>
    {
        public UpdateCityValidator()
        {
            RuleFor(x => x.CityID).NotEmpty().WithMessage("City ID is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters")
                .MaximumLength(15).WithMessage("Name must not exceed 15 characters");
        }
    }
}
