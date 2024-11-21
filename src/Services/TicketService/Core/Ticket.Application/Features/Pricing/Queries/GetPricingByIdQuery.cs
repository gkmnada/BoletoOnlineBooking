using MediatR;
using Ticket.Application.Features.Pricing.Results;

namespace Ticket.Application.Features.Pricing.Queries
{
    public class GetPricingByIdQuery : IRequest<GetPricingByIdQueryResult>
    {
        public string pricing_id { get; set; }

        public GetPricingByIdQuery(string id)
        {
            pricing_id = id;
        }
    }
}
