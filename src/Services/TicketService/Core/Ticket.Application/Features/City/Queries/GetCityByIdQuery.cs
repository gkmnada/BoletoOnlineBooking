using MediatR;
using Ticket.Application.Features.City.Results;

namespace Ticket.Application.Features.City.Queries
{
    public class GetCityByIdQuery : IRequest<GetCityByIdQueryResult>
    {
        public string CityID { get; set; }

        public GetCityByIdQuery(string id)
        {
            CityID = id;
        }
    }
}
