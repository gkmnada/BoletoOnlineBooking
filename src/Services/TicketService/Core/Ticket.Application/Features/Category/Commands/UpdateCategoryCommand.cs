using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Category.Commands
{
    public class UpdateCategoryCommand : IRequest<BaseResponse>
    {
        public string CategoryID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
