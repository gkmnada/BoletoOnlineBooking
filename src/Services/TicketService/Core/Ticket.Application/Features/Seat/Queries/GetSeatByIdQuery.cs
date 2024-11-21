using MediatR;
using Ticket.Application.Features.Seat.Results;

namespace Ticket.Application.Features.Seat.Queries
{
    public class GetSeatByIdQuery : IRequest<GetSeatByIdQueryResult>
    {
        public string seat_id { get; set; }

        public GetSeatByIdQuery(string id)
        {
            seat_id = id;
        }
    }
}
