using MediatR;
using Ticket.Application.Features.Pricing.Results;

namespace Ticket.Application.Features.Pricing.Queries
{
    public class GetPricingByIdQuery : IRequest<GetPricingByIdQueryResult>
    {
        public string PricingID { get; set; }

        public GetPricingByIdQuery(string id)
        {
            PricingID = id;
        }
    }
}
