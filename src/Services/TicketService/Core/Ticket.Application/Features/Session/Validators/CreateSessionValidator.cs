using FluentValidation;
using Ticket.Application.Features.Session.Commands;

namespace Ticket.Application.Features.Session.Validators
{
    public class CreateSessionValidator : AbstractValidator<CreateSessionCommand>
    {
        public CreateSessionValidator()
        {
            RuleFor(x => x.SessionDate).NotEmpty().WithMessage("Session date is required");
            RuleFor(x => x.SessionTime).NotEmpty().WithMessage("Session time is required");
            RuleFor(x => x.HallID).NotEmpty().WithMessage("Hall ID is required");
            RuleFor(x => x.CinemaID).NotEmpty().WithMessage("Cinema ID is required");
            RuleFor(x => x.MovieID).NotEmpty().WithMessage("Movie ID is required");
        }
    }
}
