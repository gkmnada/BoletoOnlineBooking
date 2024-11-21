using MediatR;
using Ticket.Application.Features.Seat.Results;

namespace Ticket.Application.Features.Seat.Queries
{
    public class GetSeatsQuery : IRequest<List<GetSeatsQueryResult>>
    {
    }
}
