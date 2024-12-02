using MediatR;
using Order.Application.Features.Order.Results;

namespace Order.Application.Features.Order.Queries
{
    public class GetOrdersQuery : IRequest<List<GetOrdersQueryResult>>
    {
    }
}
