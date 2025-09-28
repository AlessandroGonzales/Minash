using Application.DTO;

namespace Application.Interfaces
{
    public interface IPaymentAppService
    {
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
        Task<PaymentDto?> GetPaymentByIdAsync(int id);
        Task<IEnumerable<PaymentDto>> GetPaymentsByOrderIdAsync(int orderId);
        Task<PaymentDto> AddPaymentAsync(PaymentDto payment);
        Task UpdatePaymentAsync(PaymentDto payment);
        Task DeletePaymentAsync(int id);
    }
}
