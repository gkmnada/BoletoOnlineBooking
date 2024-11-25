using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Session.Commands
{
    public class DeleteSessionCommand : IRequest<BaseResponse>
    {
        public string SessionID { get; set; }

        public DeleteSessionCommand(string id)
        {
            SessionID = id;
        }
    }
}
