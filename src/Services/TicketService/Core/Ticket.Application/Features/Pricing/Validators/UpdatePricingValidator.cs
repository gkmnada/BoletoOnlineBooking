using FluentValidation;
using Ticket.Application.Features.Pricing.Commands;

namespace Ticket.Application.Features.Pricing.Validators
{
    public class UpdatePricingValidator : AbstractValidator<UpdatePricingCommand>
    {
        public UpdatePricingValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Pricing ID is required");
            RuleFor(x => x.session_id).NotEmpty().WithMessage("Session ID is required");
            RuleFor(x => x.category_id).NotEmpty().WithMessage("Category ID is required");
            RuleFor(x => x.price).NotEmpty().WithMessage("Price is required");
        }
    }
}
