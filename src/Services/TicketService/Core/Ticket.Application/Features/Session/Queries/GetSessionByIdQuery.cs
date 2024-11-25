using MediatR;
using Ticket.Application.Features.Session.Results;

namespace Ticket.Application.Features.Session.Queries
{
    public class GetSessionByIdQuery : IRequest<GetSessionByIdQueryResult>
    {
        public string SessionID { get; set; }

        public GetSessionByIdQuery(string id)
        {
            SessionID = id;
        }
    }
}
