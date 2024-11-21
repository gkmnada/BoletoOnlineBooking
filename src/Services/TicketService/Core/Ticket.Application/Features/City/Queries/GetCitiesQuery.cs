using MediatR;
using Ticket.Application.Features.City.Results;

namespace Ticket.Application.Features.City.Queries
{
    public class GetCitiesQuery : IRequest<List<GetCitiesQueryResult>>
    {
    }
}
