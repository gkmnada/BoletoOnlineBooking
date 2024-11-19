using FluentValidation;
using Ticket.Application.Features.Category.Commands;

namespace Ticket.Application.Features.Category.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Name is required");
        }
    }
}
