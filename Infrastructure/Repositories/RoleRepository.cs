using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EfRole = Infrastructure.Persistence.Entities.Role;

namespace Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly MinashDbContext _db;
        public RoleRepository(MinashDbContext db)
        {
            _db = db;
        }

        private static Role MapToDomain (EfRole efRole) => new Role
        {
            IdRole = efRole.IdRole,
            RoleName = efRole.RoleName,
            RoleDetails = efRole.RoleDetails,
            UpdatedAt = efRole.UpdatedAt.HasValue ? DateTime.SpecifyKind(efRole.UpdatedAt.Value, DateTimeKind.Utc) : DateTime.MinValue,
            CreatedAt = efRole.CreatedAt.HasValue ? DateTime.SpecifyKind(efRole.CreatedAt.Value, DateTimeKind.Utc) : DateTime.MinValue,
            
            Users = efRole.Users.Select(u => new User
            {
                IdUser = u.IdUser,
                UserName = u.UserName,
                LastName = u.LastName,
                Email = u.Email,
                PasswordHash = u.PasswordHash,
                Phone = u.Phone,
                Address = u.Address,
                CreatedAt = u.CreatedAt.HasValue ? DateTime.SpecifyKind(u.CreatedAt.Value, DateTimeKind.Utc) : DateTime.MinValue,
                UpdatedAt = u.UpdatedAt.HasValue ? DateTime.SpecifyKind(u.UpdatedAt.Value, DateTimeKind.Utc) : DateTime.MinValue,
                ImageUrl = u.ImageUrl ?? string.Empty,
                Province = u.Province ?? string.Empty,
                City = u.City ?? string.Empty,
                FullAddress = u.FullAddress ?? string.Empty,
                IdRole = u.IdRole
            }).ToList()
        };

        private static EfRole MapToEf(Role role) => new EfRole
        {
            RoleName = role.RoleName,
            RoleDetails = role.RoleDetails,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt
        };

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        { 
            var list = await _db.Roles.AsNoTracking().ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            var efRole = await _db.Roles.FindAsync(id);
            return efRole == null ? null : MapToDomain(efRole);
        }

        public async Task<IEnumerable<Role>> GetRolesByNameAsync(string name)
        { 
            var list = await _db.Roles
                .Where(r => r.RoleName.Replace(" ", "").ToLower().Contains(name))
                .AsNoTracking()
                .ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<Role> AddRoleAsync(Role role)
        {
            var efRole = MapToEf(role);
            _db.Roles.Add(efRole);
            await _db.SaveChangesAsync();
            role.IdRole = efRole.IdRole;
            return role;
        }

        public async Task UpdateRoleAsync(Role role)
        {
            var existingRole = await _db.Roles.FindAsync(role.IdRole);
            if (existingRole == null) throw new KeyNotFoundException($"Role with ID {role.IdRole} not found.");
            existingRole.RoleName = role.RoleName;
            existingRole.RoleDetails = role.RoleDetails;
            existingRole.UpdatedAt = DateTime.UtcNow;
            _db.Roles.Update(existingRole);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(int id)
        {
            var efRole = await _db.Roles.FindAsync(id);
            if (efRole == null) throw new KeyNotFoundException($"Role with ID {id} not found.");
            _db.Roles.Remove(efRole);
            await _db.SaveChangesAsync();
        }
    }
}
