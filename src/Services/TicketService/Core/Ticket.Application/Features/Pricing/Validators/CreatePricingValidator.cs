using FluentValidation;
using Ticket.Application.Features.Pricing.Commands;

namespace Ticket.Application.Features.Pricing.Validators
{
    public class CreatePricingValidator : AbstractValidator<CreatePricingCommand>
    {
        public CreatePricingValidator()
        {
            RuleFor(x => x.SessionID).NotEmpty().WithMessage("Session ID is required");
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("Category ID is required");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
        }
    }
}
