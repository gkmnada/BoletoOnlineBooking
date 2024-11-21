using MediatR;
using Ticket.Application.Features.Hall.Results;

namespace Ticket.Application.Features.Hall.Queries
{
    public class GetHallsQuery : IRequest<List<GetHallsQueryResult>>
    {
    }
}
