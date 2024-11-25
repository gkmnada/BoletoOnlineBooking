using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Cinema.Commands
{
    public class DeleteCinemaCommand : IRequest<BaseResponse>
    {
        public string CinemaID { get; set; }

        public DeleteCinemaCommand(string id)
        {
            CinemaID = id;
        }
    }
}
