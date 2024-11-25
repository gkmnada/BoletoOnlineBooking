using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Cinema.Commands
{
    public class UpdateCinemaCommand : IRequest<BaseResponse>
    {
        public string CinemaID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CityID { get; set; }
        public bool IsActive { get; set; }
    }
}
