using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Seat.Commands
{
    public class UpdateSeatCommand : IRequest<BaseResponse>
    {
        public string SeatID { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
        public string HallID { get; set; }
        public bool IsActive { get; set; }
    }
}
