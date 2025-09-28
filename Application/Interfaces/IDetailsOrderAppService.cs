using Application.DTO;

namespace Application.Interfaces
{
    public interface IDetailsOrderAppService
    {
        Task<IEnumerable<DetailsOrderDto>> GetAllDetailsOrdersAsync();
        Task<DetailsOrderDto?> GetDetailsOrderByIdAsync(int id);
        Task<IEnumerable<DetailsOrderDto>> GetDetailsOrdersByOrderIdAsync(int orderId);
        Task<DetailsOrderDto> AddDetailsOrderAsync(DetailsOrderDto detailsOrderDto);
        Task UpdateDetailsOrderAsync(DetailsOrderDto detailsOrderDto);
        Task DeleteDetailsOrderAsync(int id);
    }
}
