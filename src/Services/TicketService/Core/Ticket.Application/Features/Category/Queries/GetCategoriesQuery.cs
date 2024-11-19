using MediatR;
using Ticket.Application.Features.Category.Results;

namespace Ticket.Application.Features.Category.Queries
{
    public class GetCategoriesQuery : IRequest<List<GetCategoriesQueryResult>>
    {
    }
}
