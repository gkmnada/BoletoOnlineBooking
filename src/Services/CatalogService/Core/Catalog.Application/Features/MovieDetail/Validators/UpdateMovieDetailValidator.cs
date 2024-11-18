using Catalog.Application.Features.MovieDetail.Commands;
using FluentValidation;

namespace Catalog.Application.Features.MovieDetail.Validators
{
    public class UpdateMovieDetailValidator : AbstractValidator<UpdateMovieDetailCommand>
    {
        public UpdateMovieDetailValidator()
        {
            
        }
    }
}
