using MediatR;
using Ticket.Application.Features.City.Results;

namespace Ticket.Application.Features.City.Queries
{
    public class GetCityByIdQuery : IRequest<GetCityByIdQueryResult>
    {
        public string city_id { get; set; }

        public GetCityByIdQuery(string id)
        {
            city_id = id;
        }
    }
}
