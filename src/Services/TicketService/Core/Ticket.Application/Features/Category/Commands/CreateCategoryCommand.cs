using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Category.Commands
{
    public class CreateCategoryCommand : IRequest<BaseResponse>
    {
        public string name { get; set; }
    }
}
