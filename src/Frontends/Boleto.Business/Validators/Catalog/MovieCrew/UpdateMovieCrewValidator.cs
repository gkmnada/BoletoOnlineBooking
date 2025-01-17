﻿using Boleto.Messages.Catalog.MovieCrew.Requests;
using FluentValidation;

namespace Boleto.Business.Validators.Catalog.MovieCrew
{
    public class UpdateMovieCrewValidator : AbstractValidator<UpdateMovieCrewRequest>
    {
        public UpdateMovieCrewValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Adı Boş Bırakılamaz")
                .MinimumLength(3).WithMessage("Adı 3 Karakterden Az Olamaz")
                .MaximumLength(50).WithMessage("Adı 50 Karakterden Fazla Olamaz");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Ünvan Boş Bırakılamaz")
                .MinimumLength(3).WithMessage("Ünvan 3 Karakterden Az Olamaz")
                .MaximumLength(50).WithMessage("Ünvan 50 Karakterden Fazla Olamaz");
            RuleFor(x => x.ImageURL).NotEmpty().WithMessage("Resim Seçiniz");
        }
    }
}