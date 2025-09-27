using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class RoleAppService : IRoleAppService
    {
        private readonly IRoleRepository _repo;
        public RoleAppService(IRoleRepository repo)
        {
            _repo = repo;
        }

        private static RoleDto MaptoDto(Role d) => new RoleDto
        {
            IdRole = d.IdRole,
            RoleName = d.RoleName,
            RoleDetails = d.RoleDetails,
        };

        private static Role MaptoDomain(RoleDto dto) => new Role
        {
            IdRole = dto.IdRole,
            RoleName = dto.RoleName,
            RoleDetails = dto.RoleDetails,
        };

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var list = await _repo.GetAllRolesAsync();
            return list.Select(MaptoDto);
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int id)
        {
            var role = await _repo.GetRoleByIdAsync(id);
            return role == null ? null : MaptoDto(role);
        }

        public async Task<IEnumerable<RoleDto>> GetRolesByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<RoleDto>();
            var normalizedName = name.Replace(" ", "").ToLower();
            var list = await _repo.GetRolesByNameAsync(normalizedName);
            return list.Select(MaptoDto);
        }

        public async Task<RoleDto> AddRoleAsync(RoleDto role)
        {
            var CreateRole = MaptoDomain(role);
            var CreatedRole = await _repo.AddRoleAsync(CreateRole);
            return MaptoDto(CreatedRole);
        }

        public async Task UpdateRoleAsync(RoleDto role)
        {
            var domainRole = MaptoDomain(role);
            await _repo.UpdateRoleAsync(domainRole);
        }

        public async Task DeleteRoleAsync(int id)
        {
            await _repo.DeleteRoleAsync(id);
        }
    }
}
