using Catalog.Application.Features.MovieCast.Commands;
using FluentValidation;

namespace Catalog.Application.Features.MovieCast.Validators
{
    public class UpdateMovieCastValidator : AbstractValidator<UpdateMovieCastCommand>
    {
        public UpdateMovieCastValidator()
        {
            RuleFor(x => x.CastID).NotEmpty().WithMessage("Cast ID is required");
            RuleFor(x => x.CastName).NotEmpty().WithMessage("Cast name is required")
                .MinimumLength(3).WithMessage("Name must not be less than 3 characters.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            RuleFor(x => x.Character).NotEmpty().WithMessage("Character is required")
                .MinimumLength(3).WithMessage("Name must not be less than 3 characters.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            RuleFor(x => x.ImageURL).NotEmpty().WithMessage("Image URL is required");
            RuleFor(x => x.MovieID).NotEmpty().WithMessage("Movie ID is required");
        }
    }
}
