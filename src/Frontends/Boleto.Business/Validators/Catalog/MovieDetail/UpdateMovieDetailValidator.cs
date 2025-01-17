using Boleto.Messages.Catalog.MovieDetail.Requests;
using FluentValidation;

namespace Boleto.Business.Validators.Catalog.MovieDetail
{
    public class UpdateMovieDetailValidator : AbstractValidator<UpdateMovieDetailRequest>
    {
        public UpdateMovieDetailValidator()
        {
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.ImageURL == null && string.IsNullOrEmpty(x.ExistingImageURL))
                {
                    context.AddFailure("ImageURL", "Kapak Görseli Seçiniz");
                }
            });
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.VideoURL == null && string.IsNullOrEmpty(x.ExistingVideoURL))
                {
                    context.AddFailure("VideoURL", "Film Fragmanı Seçiniz");
                }
            });
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama Boş Bırakılamaz")
                .MinimumLength(3).WithMessage("Açıklama 3 Karakterden Az Olamaz")
                .MaximumLength(250).WithMessage("Açıklama 250 Karakterden Fazla Olamaz");
        }
    }
}
