using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Hall.Commands
{
    public class DeleteHallCommand : IRequest<BaseResponse>
    {
        public string hall_id { get; set; }

        public DeleteHallCommand(string id)
        {
            hall_id = id;
        }
    }
}
