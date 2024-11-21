using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Cinema.Commands
{
    public class UpdateCinemaCommand : IRequest<BaseResponse>
    {
        public string id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string city_id { get; set; }
        public bool is_active { get; set; }
    }
}
