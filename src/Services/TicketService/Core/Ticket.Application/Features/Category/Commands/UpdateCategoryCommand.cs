using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Category.Commands
{
    public class UpdateCategoryCommand : IRequest<BaseResponse>
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool is_active { get; set; }
    }
}
