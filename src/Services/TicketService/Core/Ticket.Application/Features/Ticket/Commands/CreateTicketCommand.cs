using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Ticket.Commands
{
    public class CreateTicketCommand : IRequest<BaseResponse>
    {
        public string SessionID { get; set; }
        public string SeatID { get; set; }
        public decimal Price { get; set; }
    }
}
