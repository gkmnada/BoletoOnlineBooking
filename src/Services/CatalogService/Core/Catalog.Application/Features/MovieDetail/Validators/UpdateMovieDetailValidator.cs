using Catalog.Application.Features.MovieDetail.Commands;
using FluentValidation;

namespace Catalog.Application.Features.MovieDetail.Validators
{
    public class UpdateMovieDetailValidator : AbstractValidator<UpdateMovieDetailCommand>
    {
        public UpdateMovieDetailValidator()
        {
            RuleFor(x => x.DetailID).NotEmpty().WithMessage("Detail ID is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required")
                .MinimumLength(3).WithMessage("Description must be at least 3 characters")
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters");
            RuleFor(x => x.MovieID).NotEmpty().WithMessage("Movie ID is required");
        }
    }
}
