using MediatR;
using Ticket.Application.Features.Pricing.Results;

namespace Ticket.Application.Features.Pricing.Queries
{
    public class GetPricingsQuery : IRequest<List<GetPricingsQueryResult>>
    {
    }
}
