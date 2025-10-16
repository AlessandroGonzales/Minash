using Application.DTO.Partial;
using Application.DTO.Request;

namespace Application.Interfaces
{
    public interface IPaymentAppService
    {
        Task<IEnumerable<PaymentRequest>> GetAllPaymentsAsync();
        Task<PaymentRequest?> GetPaymentByIdAsync(int id);
        Task<PaymentRequest?> GetPaymentsByOrderIdAsync(int orderId);
        Task<PaymentRequest> AddPaymentAsync(PaymentRequest payment);
        Task UpdatePaymentAsync(int id, PaymentRequest payment);
        Task PartialUpdatePaymentAsync(int id, PaymentPartial payment);
        Task DeletePaymentAsync(int id);
    }
}
