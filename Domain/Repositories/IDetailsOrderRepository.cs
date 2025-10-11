using Domain.Entities;

namespace Domain.Repositories
{
    public interface IDetailsOrderRepository
    {
        Task<IEnumerable<DetailsOrder>> GetAllDetailsOrdersAsync();
        Task<DetailsOrder?> GetDetailsOrderByIdAsync(int id);
        Task<IEnumerable<DetailsOrder>> GetDetailsOrdersByOrderIdAsync(int orderId);
        Task<DetailsOrder> AddDetailsOrderAsync(DetailsOrder detailsOrder);
        Task UpdateDetailsOrderAsync(int id, DetailsOrder detailsOrder);
        Task DeleteDetailsOrderAsync(int id);
    }
}
