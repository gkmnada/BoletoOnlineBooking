using Boleto.Messages.Catalog.Category.Requests;
using FluentValidation;

namespace Boleto.Business.Validators.Catalog.Category
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori Adı Boş Bırakılamaz")
                .MinimumLength(3).WithMessage("Kategori Adı 3 Karakterden Az Olamaz")
                .MaximumLength(50).WithMessage("Kategori Adı 50 Karakterden Fazla Olamaz");
            RuleFor(x => x.SlugURL).NotEmpty().WithMessage("Kategori URL Boş Bırakılamaz")
                .MinimumLength(3).WithMessage("Kategori URL 3 Karakterden Az Olamaz")
                .MaximumLength(50).WithMessage("Kategori URL 50 Karakterden Fazla Olamaz");
        }
    }
}
