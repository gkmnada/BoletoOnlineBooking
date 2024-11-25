using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Hall.Commands
{
    public class CreateHallCommand : IRequest<BaseResponse>
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string CinemaID { get; set; }
    }
}