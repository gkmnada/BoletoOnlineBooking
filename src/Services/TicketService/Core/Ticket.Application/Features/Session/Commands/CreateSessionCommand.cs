using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Session.Commands
{
    public class CreateSessionCommand : IRequest<BaseResponse>
    {
        public DateOnly SessionDate { get; set; }
        public TimeOnly SessionTime { get; set; }
        public string HallID { get; set; }
        public string CinemaID { get; set; }
        public string MovieID { get; set; }
    }
}
