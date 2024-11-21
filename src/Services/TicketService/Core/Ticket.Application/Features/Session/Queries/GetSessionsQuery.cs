using MediatR;
using Ticket.Application.Features.Session.Results;

namespace Ticket.Application.Features.Session.Queries
{
    public class GetSessionsQuery : IRequest<List<GetSessionsQueryResult>>
    {
    }
}
