using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Seat.Commands
{
    public class DeleteSeatCommand : IRequest<BaseResponse>
    {
        public string seat_id { get; set; }

        public DeleteSeatCommand(string id)
        {
            seat_id = id;
        }
    }
}
