using Catalog.Application.Features.Category.Commands;
using FluentValidation;

namespace Catalog.Application.Features.Category.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must not be less than 3 characters")
                .MaximumLength(50).WithMessage("Name must not be more than 50 characters");
            RuleFor(x => x.SlugURL).NotEmpty().WithMessage("Slug is required")
                .MinimumLength(3).WithMessage("Slug must not be less than 3 characters")
                .MaximumLength(50).WithMessage("Slug must not be more than 50 characters");
        }
    }
}
