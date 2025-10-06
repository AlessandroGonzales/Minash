using Application.DTO.Request;

namespace Application.Interfaces
{
    public interface IPaymentAppService
    {
        Task<IEnumerable<PaymentRequest>> GetAllPaymentsAsync();
        Task<PaymentRequest?> GetPaymentByIdAsync(int id);
        Task<IEnumerable<PaymentRequest>> GetPaymentsByOrderIdAsync(int orderId);
        Task<PaymentRequest> AddPaymentAsync(PaymentRequest payment);
        Task UpdatePaymentAsync(PaymentRequest payment);
        Task DeletePaymentAsync(int id);
    }
}
