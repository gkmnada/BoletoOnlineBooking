using Catalog.Application.Features.MovieImage.Commands;
using FluentValidation;

namespace Catalog.Application.Features.MovieImage.Validators
{
    public class UpdateMovieImageValidator : AbstractValidator<UpdateMovieImageCommand>
    {
        public UpdateMovieImageValidator()
        {
            RuleFor(x => x.ImageURL).NotEmpty().WithMessage("Image URL is required");
            RuleFor(x => x.MovieID).NotEmpty().WithMessage("Movie ID is required");
        }
    }
}
