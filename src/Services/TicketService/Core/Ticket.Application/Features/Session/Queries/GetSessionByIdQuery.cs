using MediatR;
using Ticket.Application.Features.Session.Results;

namespace Ticket.Application.Features.Session.Queries
{
    public class GetSessionByIdQuery : IRequest<GetSessionByIdQueryResult>
    {
        public string session_id { get; set; }

        public GetSessionByIdQuery(string id)
        {
            session_id = id;
        }
    }
}
