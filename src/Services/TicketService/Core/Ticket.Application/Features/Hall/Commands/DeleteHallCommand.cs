using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Hall.Commands
{
    public class DeleteHallCommand : IRequest<BaseResponse>
    {
        public string HallID { get; set; }

        public DeleteHallCommand(string id)
        {
            HallID = id;
        }
    }
}
