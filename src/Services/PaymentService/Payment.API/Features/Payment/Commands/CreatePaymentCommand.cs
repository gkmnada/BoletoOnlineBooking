using MediatR;
using Payment.API.Common.Base;

namespace Payment.API.Features.Payment.Commands
{
    public class CreatePaymentCommand : IRequest<BaseResponse>
    {
        public string ConversationID { get; set; }
        public string ProcessID { get; set; }
        public List<string> TransactionsID { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentAmount { get; set; }
        public string PaymentType { get; set; }
        public string OrderID { get; set; }
    }
}
