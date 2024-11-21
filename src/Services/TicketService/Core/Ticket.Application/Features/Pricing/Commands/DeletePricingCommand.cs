using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Pricing.Commands
{
    public class DeletePricingCommand : IRequest<BaseResponse>
    {
        public string pricing_id { get; set; }

        public DeletePricingCommand(string id)
        {
            pricing_id = id;
        }
    }
}
