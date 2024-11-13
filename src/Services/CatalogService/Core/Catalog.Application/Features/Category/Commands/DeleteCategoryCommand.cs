using Catalog.Application.Common.Base;
using MediatR;

namespace Catalog.Application.Features.Category.Commands
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
