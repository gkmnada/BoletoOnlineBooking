using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Cinema.Commands
{
    public class DeleteCinemaCommand : IRequest<BaseResponse>
    {
        public string cinema_id { get; set; }

        public DeleteCinemaCommand(string id)
        {
            cinema_id = id;
        }
    }
}
