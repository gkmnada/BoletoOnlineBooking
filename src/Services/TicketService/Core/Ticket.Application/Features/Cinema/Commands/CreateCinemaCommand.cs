using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Cinema.Commands
{
    public class CreateCinemaCommand : IRequest<BaseResponse>
    {
        public string name { get; set; }
        public string address { get; set; }
        public string city_id { get; set; }
    }
}
