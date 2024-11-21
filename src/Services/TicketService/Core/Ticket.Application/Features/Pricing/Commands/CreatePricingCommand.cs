using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Pricing.Commands
{
    public class CreatePricingCommand : IRequest<BaseResponse>
    {
        public string session_id { get; set; }
        public string category_id { get; set; }
        public decimal price { get; set; }
    }
}
