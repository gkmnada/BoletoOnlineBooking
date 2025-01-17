using Boleto.Messages.Catalog.MovieCast.Requests;
using FluentValidation;

namespace Boleto.Business.Validators.Catalog.MovieCast
{
    public class CreateMovieCastValidator : AbstractValidator<CreateMovieCastRequest>
    {
        public CreateMovieCastValidator()
        {
            RuleForEach(x => x.MovieCasts).SetValidator(new CreateMovieCastItemValidator());
        }
    }

    public class CreateMovieCastItemValidator : AbstractValidator<CreateMovieCastItem>
    {
        public CreateMovieCastItemValidator()
        {
            RuleFor(x => x.CastName).NotEmpty().WithMessage("Oyuncu Adı Boş Bırakılamaz")
                .MinimumLength(3).WithMessage("Oyuncu Adı 3 Karakterden Az Olamaz")
                .MaximumLength(50).WithMessage("Oyuncu Adı 50 Karakterden Fazla Olamaz");
            RuleFor(x => x.Character).NotEmpty().WithMessage("Karakter Adı Boş Bırakılamaz")
                .MinimumLength(3).WithMessage("Karakter Adı 3 Karakterden Az Olamaz")
                .MaximumLength(50).WithMessage("Karakter Adı 50 Karakterden Fazla Olamaz");
            RuleFor(x => x.ImageURL).NotEmpty().WithMessage("Resim Seçiniz");
        }
    }
}
