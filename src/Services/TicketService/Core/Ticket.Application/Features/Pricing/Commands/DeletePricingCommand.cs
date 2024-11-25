using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Pricing.Commands
{
    public class DeletePricingCommand : IRequest<BaseResponse>
    {
        public string PricingID { get; set; }

        public DeletePricingCommand(string id)
        {
            PricingID = id;
        }
    }
}
