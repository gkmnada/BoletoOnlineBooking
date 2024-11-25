using MediatR;
using Ticket.Application.Features.Category.Results;

namespace Ticket.Application.Features.Category.Queries
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
