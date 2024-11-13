using Catalog.Application.Features.Category.Results;
using MediatR;

namespace Catalog.Application.Features.Category.Queries
{
    public class GetCategoryByIdQuery : IRequest<GetCategoryByIdQueryResult>
    {
        public string CategoryID { get; set; }

        public GetCategoryByIdQuery(string id)
        {
            CategoryID = id;
        }
    }
}
