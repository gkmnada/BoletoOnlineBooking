using Catalog.Application.Features.Movie.Commands;
using FluentValidation;

namespace Catalog.Application.Features.Movie.Validators
{
    public class UpdateMovieValidator : AbstractValidator<UpdateMovieCommand>
    {
        public UpdateMovieValidator()
        {
            RuleFor(x => x.MovieID).NotEmpty().WithMessage("Movie ID is required");
            RuleFor(x => x.MovieName).NotEmpty().WithMessage("Movie name is required")
                .MinimumLength(3).WithMessage("Movie name must not be less than 3 characters")
                .MaximumLength(50).WithMessage("Movie name must not be more than 50 characters");
            RuleFor(x => x.Genre).NotEmpty().WithMessage("Genre is required");
            RuleFor(x => x.Language).NotEmpty().WithMessage("Language is required");
            RuleFor(x => x.Duration).NotEmpty().WithMessage("Duration is required");
            RuleFor(x => x.ReleaseDate).NotEmpty().WithMessage("Release date is required");
            RuleFor(x => x.Rating).NotEmpty().WithMessage("Rating is required");
            RuleFor(x => x.AudienceScore).NotEmpty().WithMessage("Audience score is required");
            RuleFor(x => x.ImageURL).NotEmpty().WithMessage("Image URL is required");
            RuleFor(x => x.SlugURL).NotEmpty().WithMessage("Slug URL is required");
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("Category ID is required");
        }
    }
}
