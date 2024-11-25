using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Seat.Commands
{
    public class DeleteSeatCommand : IRequest<BaseResponse>
    {
        public string SeatID { get; set; }

        public DeleteSeatCommand(string id)
        {
            SeatID = id;
        }
    }
}
