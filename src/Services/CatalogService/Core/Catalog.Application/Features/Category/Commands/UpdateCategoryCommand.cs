using Catalog.Application.Common.Base;
using MediatR;

namespace Catalog.Application.Features.Category.Commands
{
    public class UpdateCategoryCommand : IRequest<BaseResponse>
    {
        public string CategoryID { get; set; }
        public string Name { get; set; }
        public string SlugURL { get; set; }
        public bool IsActive { get; set; }
    }
}
