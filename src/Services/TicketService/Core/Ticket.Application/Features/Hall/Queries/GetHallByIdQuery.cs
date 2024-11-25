using MediatR;
using Ticket.Application.Features.Hall.Results;

namespace Ticket.Application.Features.Hall.Queries
{
    public class GetHallByIdQuery : IRequest<GetHallByIdQueryResult>
    {
        public string HallID { get; set; }

        public GetHallByIdQuery(string id)
        {
            HallID = id;
        }
    }
}
