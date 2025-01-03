using Boleto.Messages.Catalog.Movie.Requests;
using FluentValidation;

namespace Boleto.Business.Validators.Catalog.Movie
{
    public class CreateMovieValidator : AbstractValidator<CreateMovieRequest>
    {
        public CreateMovieValidator()
        {
            RuleFor(x => x.MovieName).NotEmpty().WithMessage("Film Adı Boş Bırakılamaz")
                .MinimumLength(3).WithMessage("Film Adı 3 Karakterden Az Olamaz")
                .MaximumLength(50).WithMessage("Film Adı 50 Karakterden Fazla Olamaz");
            RuleFor(x => x.Genre).NotEmpty().WithMessage("Tür Boş Bırakılamaz");
            RuleFor(x => x.Language).NotEmpty().WithMessage("Dil Boş Bırakılamaz");
            RuleFor(x => x.Duration).NotEmpty().WithMessage("Süre Boş Bırakılamaz");
            RuleFor(x => x.ReleaseDate).NotEmpty().WithMessage("Yayın Tarihi Boş Bırakılamaz");
            RuleFor(x => x.Rating).NotEmpty().WithMessage("Puan Boş Bırakılamaz");
            RuleFor(x => x.AudienceScore).NotEmpty().WithMessage("İzleyici Puanı Boş Bırakılamaz");
            RuleFor(x => x.ImageURL).NotEmpty().WithMessage("Resim Seçiniz");
            RuleFor(x => x.SlugURL).NotEmpty().WithMessage("Film URL Boş Bırakılamaz")
                .MinimumLength(3).WithMessage("Film URL 3 Karakterden Az Olamaz")
                .MaximumLength(50).WithMessage("Film URL 50 Karakterden Fazla Olamaz");
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("Kategori ID Boş Bırakılamaz");
        }
    }
}
