using MediatR;
using Ticket.Application.Features.Hall.Results;

namespace Ticket.Application.Features.Hall.Queries
{
    public class GetHallByIdQuery : IRequest<GetHallByIdQueryResult>
    {
        public string hall_id { get; set; }

        public GetHallByIdQuery(string id)
        {
            hall_id = id;
        }
    }
}
