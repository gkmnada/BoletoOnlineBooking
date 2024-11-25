using MediatR;
using Ticket.Application.Features.Seat.Results;

namespace Ticket.Application.Features.Seat.Queries
{
    public class GetSeatByIdQuery : IRequest<GetSeatByIdQueryResult>
    {
        public string SeatID { get; set; }

        public GetSeatByIdQuery(string id)
        {
            SeatID = id;
        }
    }
}
