using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Session.Commands
{
    public class DeleteSessionCommand : IRequest<BaseResponse>
    {
        public string session_id { get; set; }

        public DeleteSessionCommand(string id)
        {
            session_id = id;
        }
    }
}
