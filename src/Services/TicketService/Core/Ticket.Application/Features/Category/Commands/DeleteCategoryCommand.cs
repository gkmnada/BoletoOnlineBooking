using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Category.Commands
{
    public class DeleteCategoryCommand : IRequest<BaseResponse>
    {
        public string CategoryID { get; set; }

        public DeleteCategoryCommand(string id)
        {
            CategoryID = id;
        }
    }
}
