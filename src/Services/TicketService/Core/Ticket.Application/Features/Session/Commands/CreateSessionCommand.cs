using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Session.Commands
{
    public class CreateSessionCommand : IRequest<BaseResponse>
    {
        public DateOnly session_date { get; set; }
        public TimeOnly session_time { get; set; }
        public string hall_id { get; set; }
        public string cinema_id { get; set; }
        public string movie_id { get; set; }
    }
}
