using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Pricing.Commands
{
    public class UpdatePricingCommand : IRequest<BaseResponse>
    {
        public string PricingID { get; set; }
        public string SessionID { get; set; }
        public string CategoryID { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
