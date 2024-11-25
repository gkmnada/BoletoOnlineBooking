using Payment.API.Common.Base;

namespace Payment.API.Repositories.Payment
{
    public interface IPaymentRepository
    {
        Task CreatePaymentAsync(Entities.Payment payment);
    }
}
