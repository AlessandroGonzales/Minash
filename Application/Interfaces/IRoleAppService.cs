using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IRoleAppService
    {
        Task<IEnumerable<RoleResponse>> GetAllRolesAsync();
        Task<RoleResponse?> GetRoleByIdAsync(int id);
        Task<IEnumerable<RoleResponse>> GetRolesByNameAsync(string name);
        Task<RoleResponse> AddRoleAsync(RoleRequest roleDto);
        Task UpdateRoleAsync(int id, RoleRequest roleDto);
        Task PartialUpdateRoleAsync(int id, RolPartial roleDto);
        Task DeleteRoleAsync(int id);
    }
}
