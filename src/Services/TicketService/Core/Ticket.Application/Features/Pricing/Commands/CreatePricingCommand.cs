using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Pricing.Commands
{
    public class CreatePricingCommand : IRequest<BaseResponse>
    {
        public string SessionID { get; set; }
        public string CategoryID { get; set; }
        public decimal Price { get; set; }
    }
}
