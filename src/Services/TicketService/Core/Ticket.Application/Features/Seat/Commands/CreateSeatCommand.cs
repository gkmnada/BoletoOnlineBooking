using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Seat.Commands
{
    public class CreateSeatCommand : IRequest<BaseResponse>
    {
        public string row { get; set; }
        public int number { get; set; }
        public string hall_id { get; set; }
    }
}
