using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Cinema.Commands
{
    public class CreateCinemaCommand : IRequest<BaseResponse>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string CityID { get; set; }
    }
}
