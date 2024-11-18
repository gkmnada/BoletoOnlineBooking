using Catalog.Application.Features.MovieDetail.Commands;
using FluentValidation;

namespace Catalog.Application.Features.MovieDetail.Validators
{
    public class CreateMovieDetailValidator : AbstractValidator<CreateMovieDetailCommand>
    {
        public CreateMovieDetailValidator()
        {
            
        }
    }
}
