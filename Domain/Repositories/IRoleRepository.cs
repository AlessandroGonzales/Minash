using Domain.Entities;
namespace Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int id);
        Task<IEnumerable<Role>> GetRolesByNameAsync(string name);
        Task<Role> AddRoleAsync(Role role);
        Task UpdateRoleAsync(int id, Role role);
        Task PartialUpdateRoleAsync(int id, Role role);
        Task DeleteRoleAsync(int id);

    }
}
