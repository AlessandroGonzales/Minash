using Domain.Entities;

namespace Domain.Repositories
{
    public interface IGarmentRepository
    {
        Task<IEnumerable<Garment>> GetAllGarmentsAsync();
        Task<Garment?> GetGarmentByIdAsync(int id);
        Task<IEnumerable<Garment>> GetGarmentsByNameAsync(string name);
        Task<Garment> AddGarmentAsync(Garment garment);
        Task UpdateGarmentAsync(Garment garment);
        Task DeleteGarmentAsync(int id);
    }
}
