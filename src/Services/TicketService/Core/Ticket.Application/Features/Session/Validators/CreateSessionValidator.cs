using FluentValidation;
using Ticket.Application.Features.Session.Commands;

namespace Ticket.Application.Features.Session.Validators
{
    public class CreateSessionValidator : AbstractValidator<CreateSessionCommand>
    {
        public CreateSessionValidator()
        {
            RuleFor(x => x.session_date).NotEmpty().WithMessage("Session date is required");
            RuleFor(x => x.hall_id).NotEmpty().WithMessage("Hall ID is required");
            RuleFor(x => x.cinema_id).NotEmpty().WithMessage("Cinema ID is required");
            RuleFor(x => x.movie_id).NotEmpty().WithMessage("Movie ID is required");
        }
    }
}
