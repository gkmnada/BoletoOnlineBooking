using MediatR;
using Ticket.Application.Common.Base;

namespace Ticket.Application.Features.Pricing.Commands
{
    public class UpdatePricingCommand : IRequest<BaseResponse>
    {
        public string id { get; set; }
        public string session_id { get; set; }
        public string category_id { get; set; }
        public decimal price { get; set; }
        public bool is_active { get; set; }
    }
}
