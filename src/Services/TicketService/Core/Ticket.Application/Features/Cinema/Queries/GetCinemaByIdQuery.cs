using MediatR;
using Ticket.Application.Features.Cinema.Results;

namespace Ticket.Application.Features.Cinema.Queries
{
    public class GetCinemaByIdQuery : IRequest<GetCinemaByIdQueryResult>
    {
        public string CinemaID { get; set; }

        public GetCinemaByIdQuery(string id)
        {
            CinemaID = id;
        }
    }
}
