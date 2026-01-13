using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IDetailsOrderAppService
    {
        Task<IEnumerable<DetailsOrderResponse>> GetAllDetailsOrdersAsync();
        Task<IEnumerable<DetailsOrderResponse>> GetDetailsOrdersByOrderIdAsync(int orderId);
        Task<DetailsOrderResponse?> GetDetailsOrderByIdAsync(int id);
        Task<DetailsOrderResponse> AddDetailsOrderAsync(int userId, DetailsOrderRequest detailsOrderDto);
        Task UpdateDetailsOrderAsync(int id, DetailsOrderRequest detailsOrderDto);
        Task DeleteDetailsOrderAsync(int id);
    }
}
