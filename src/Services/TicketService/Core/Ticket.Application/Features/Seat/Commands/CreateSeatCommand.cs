using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Seat.Commands
{
    public class CreateSeatCommand : IRequest<BaseResponse>
    {
        public string Row { get; set; }
        public int Number { get; set; }
        public string HallID { get; set; }
    }
}
