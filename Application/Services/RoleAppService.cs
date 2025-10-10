using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;
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

        private static RoleResponse MapToResponse(Role d) => new RoleResponse
        {
            IdRol = d.IdRole,
            RoleName = d.RoleName,
            RoleDetails = d.RoleDetails,
        };

        private static Role MaptoDomain(RoleRequest dto) => new Role
        {
            IdRole = dto.IdRole,
            RoleName = dto.RoleName,
            RoleDetails = dto.RoleDetails,
        };

        private static Role MapToDomain(RolPartial dto) => new Role
        {
            RoleName = dto.RolName,
            RoleDetails = dto.RolDetails,
        };

        public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync()
        {
            var list = await _repo.GetAllRolesAsync();
            return list.Select(MapToResponse);
        }

        public async Task<RoleResponse?> GetRoleByIdAsync(int id)
        {
            var role = await _repo.GetRoleByIdAsync(id);
            return role == null ? null : MapToResponse(role);
        }

        public async Task<IEnumerable<RoleResponse>> GetRolesByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<RoleResponse>();
            var normalizedName = name.Replace(" ", "").ToLower();
            var list = await _repo.GetRolesByNameAsync(normalizedName);
            return list.Select(MapToResponse);
        }

        public async Task<RoleResponse> AddRoleAsync(RoleRequest role)
        {
            var CreateRole = MaptoDomain(role);
            var CreatedRole = await _repo.AddRoleAsync(CreateRole);
            return MapToResponse(CreatedRole);
        }

        public async Task UpdateRoleAsync(int id, RoleRequest role)
        {
            var domainRole = MaptoDomain(role);
            await _repo.UpdateRoleAsync(id, domainRole);
        }
        
        public async Task PartialUpdateRoleAsync(int id, RolPartial role)
        {
            var domainRole = MapToDomain(role);
            await _repo.PartialUpdateRoleAsync(id, domainRole);
        }

        public async Task DeleteRoleAsync(int id)
        {
            await _repo.DeleteRoleAsync(id);
        }
    }
}
