using Application.DTO;

namespace Application.Interfaces
{
    public interface IRoleAppService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<IEnumerable<RoleDto>> GetRolesByNameAsync(string name);
        Task<RoleDto> AddRoleAsync(RoleDto roleDto);
        Task UpdateRoleAsync(RoleDto roleDto);
        Task DeleteRoleAsync(int id);
    }
}
