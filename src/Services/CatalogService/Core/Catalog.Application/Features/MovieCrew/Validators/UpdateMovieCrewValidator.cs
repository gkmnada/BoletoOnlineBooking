using Catalog.Application.Features.MovieCrew.Commands;
using FluentValidation;

namespace Catalog.Application.Features.MovieCrew.Validators
{
    public class UpdateMovieCrewValidator : AbstractValidator<UpdateMovieCrewCommand>
    {
        public UpdateMovieCrewValidator()
        {
            RuleFor(x => x.CrewID).NotEmpty().WithMessage("Crew ID is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must not be less than 3 characters.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required")
                .MinimumLength(3).WithMessage("Name must not be less than 3 characters.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            RuleFor(x => x.ImageURL).NotEmpty().WithMessage("Image URL is required");
            RuleFor(x => x.MovieID).NotEmpty().WithMessage("Movie ID is required");
        }
    }
}
