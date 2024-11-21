using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Hall.Commands
{
    public class CreateHallCommand : IRequest<BaseResponse>
    {
        public string name { get; set; }
        public int capacity { get; set; }
        public string cinema_id { get; set; }
    }
}