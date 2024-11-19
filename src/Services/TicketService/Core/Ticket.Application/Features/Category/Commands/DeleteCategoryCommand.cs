using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Category.Commands
{
    public class DeleteCategoryCommand : IRequest<BaseResponse>
    {
        public string category_id { get; set; }

        public DeleteCategoryCommand(string id)
        {
            category_id = id;
        }
    }
}
