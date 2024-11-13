using Catalog.Application.Features.Category.Results;
using MediatR;

namespace Catalog.Application.Features.Category.Queries
{
    public class GetCategoriesQuery : IRequest<List<GetCategoriesQueryResult>>
    {
    }
}
