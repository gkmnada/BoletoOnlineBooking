using MediatR;
using Ticket.Application.Features.Cinema.Results;

namespace Ticket.Application.Features.Cinema.Queries
{
    public class GetCinemasQuery : IRequest<List<GetCinemasQueryResult>>
    {
    }
}
