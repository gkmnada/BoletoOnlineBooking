using FluentValidation;
using Ticket.Application.Features.Category.Commands;

namespace Ticket.Application.Features.Category.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("Category ID is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        }
    }
}
