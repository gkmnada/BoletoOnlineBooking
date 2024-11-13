using Catalog.Application.Common.Base;
using MediatR;

namespace Catalog.Application.Features.Category.Commands
{
    public class CreateCategoryCommand : IRequest<BaseResponse>
    {
        public string Name { get; set; }
        public string SlugURL { get; set; }
    }
}
