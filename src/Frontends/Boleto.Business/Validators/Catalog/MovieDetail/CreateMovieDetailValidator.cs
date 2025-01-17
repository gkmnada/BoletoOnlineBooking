using Boleto.Messages.Catalog.MovieDetail.Requests;
using FluentValidation;

namespace Boleto.Business.Validators.Catalog.MovieDetail
{
    public class CreateMovieDetailValidator : AbstractValidator<CreateMovieDetailRequest>
    {
        public CreateMovieDetailValidator()
        {
            RuleFor(x => x.ImageURL).NotEmpty().WithMessage("Kapak Görseli Seçiniz");
            RuleFor(x => x.VideoURL).NotEmpty().WithMessage("Film Fragmanı Seçiniz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama Boş Bırakılamaz")
                .MinimumLength(3).WithMessage("Açıklama 3 Karakterden Az Olamaz")
                .MaximumLength(250).WithMessage("Açıklama 250 Karakterden Fazla Olamaz");
        }
    }
}