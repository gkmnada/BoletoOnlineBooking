using Payment.API.Common.Base;
using Payment.API.Models;

namespace Payment.API.Services
{
    public interface IPaymentService
    {
        Task<BaseResponse> ProcessPaymentAsync(PaymentForm paymentForm);
    }
}
