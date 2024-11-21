using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Seat.Commands
{
    public class UpdateSeatCommand : IRequest<BaseResponse>
    {
        public string id { get; set; }
        public string row { get; set; }
        public int number { get; set; }
        public string hall_id { get; set; }
        public bool is_active { get; set; }
    }
}
