using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId);
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<Payment> AddPaymentAsync(Payment payment);
        Task UpdatePaymentAsync(int id, Payment payment);
        Task DeletePaymentAsync(int id);
    }
}
