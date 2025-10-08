using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EfRole = Infrastructure.Persistence.Entities.Role;
using EfUser = Infrastructure.Persistence.Entities.User;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MinashDbContext _db;
        public UserRepository(MinashDbContext db)
        {
            _db = db;
        }

        private static Role MapToDomainRole(EfRole efRole)
        {
            if (efRole == null) return null!;
            return new Role
            {
                IdRole = efRole.IdRole,
                RoleName = efRole.RoleName,
                RoleDetails = efRole.RoleDetails,
                CreatedAt = efRole.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = efRole.UpdatedAt ?? DateTime.UtcNow,
            };
        }

        private static User MapToDomain(EfUser ef) => new User
        {
            IdUser = ef.IdUser,
            UserName = ef.UserName,
            LastName = ef.LastName,
            Email = ef.Email,
            PasswordHash = ef.PasswordHash,
            Phone = ef.Phone,
            Address = ef.Address,
            CreatedAt = ef.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = ef.UpdatedAt ?? DateTime.UtcNow,
            ImageUrl = ef.ImageUrl ?? string.Empty,
            Province = ef.Province ?? string.Empty,
            City = ef.City ?? string.Empty,
            FullAddress = ef.FullAddress ?? string.Empty,
            IdRole = ef.IdRole,
            Role = MapToDomainRole(ef.IdRoleNavigation),

            Customs = ef.Customs.Select(c => new Custom
            {
                IdCustom = c.IdCustom,
                CustomerDetails = c.CustomerDetails,
                Count = c.Count,
                ImageUrl = c.ImageUrl ?? string.Empty,
                CreatedAt = c.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = c.UpdatedAt ?? DateTime.UtcNow,
                IdGarment = c.IdGarment,
                IdUser = c.IdUser,
                IdService = c.IdService
            }).ToList(),

            Orders = ef.Orders.Select(o => new Order
            {
                IdOrder = o.IdOrder,
                Total = o.Total,
                CreatedAt = o.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = o.UpdatedAt ?? DateTime.UtcNow,
                IdUser = o.IdUser
            }).ToList()
        };

        private static EfUser MapToEf(User user)
        {
            if (user == null) return null!;
            return new EfUser
            {
                UserName = user.UserName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Phone = user.Phone,
                Address = user.Address,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                ImageUrl = user.ImageUrl,
                Province = user.Province,
                City = user.City,
                FullAddress = user.FullAddress,
                IdRole = user.IdRole,
            };
        }

        private IQueryable<EfUser> GetQueryableWithIncludes(bool tracking = false)
        {
            var query = _db.Users
                .Include(u => u.IdRoleNavigation);
            return tracking ? query : query.AsNoTracking();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var list = await GetQueryableWithIncludes().ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<User?> GetUserByNameAsync(string name)
        {
            var user = await GetQueryableWithIncludes().FirstOrDefaultAsync(u => u.UserName == name);

            return user == null? null :MapToDomain(user);
        }

        public async Task<IEnumerable<User>> GetUsersByCityAsync(string city)
        {
            var list = await GetQueryableWithIncludes()
                .Where(u => u.City == city)
                .ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var efUser = await GetQueryableWithIncludes().FirstOrDefaultAsync(u => u.IdUser == id);
            return efUser == null ? null : MapToDomain(efUser);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var efUser = await GetQueryableWithIncludes().FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            return efUser == null ? null : MapToDomain(efUser);
        }

        public async Task<IEnumerable<User>> GetUsersByRolIdAsync(int roleId)
        {
            var list = await GetQueryableWithIncludes()
                .Where(u => u.IdRole == roleId)
                .ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<User> AddUserAsync(User user)
        {
            if (user.IdRole <= 0)
                throw new ArgumentException("Role ID must be a positive integer.");

            var efUser = MapToEf(user);
            _db.Users.Add(efUser);
            await _db.SaveChangesAsync();
            var addedUser = await GetQueryableWithIncludes().FirstOrDefaultAsync(u => u.IdUser == efUser.IdUser);
            return MapToDomain(addedUser!);
        }

        public async Task UpdateUserAsync(int id, User user)
        {
            var existingUser = await _db.Users.FindAsync(id);
            if (existingUser == null) throw new KeyNotFoundException($"User with ID {user.IdUser} not found.");
            existingUser.UserName = user.UserName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            existingUser.Address = user.Address;
            existingUser.UpdatedAt = DateTime.UtcNow;
            existingUser.ImageUrl = user.ImageUrl;
            existingUser.Province = user.Province;
            existingUser.City = user.City;
            existingUser.FullAddress = user.FullAddress;
            await _db.SaveChangesAsync();
        }

        public async Task PartialUpdateUserAsync(int id, User user)
        {
            var existingUser = await _db.Users.FindAsync(id);
            if (existingUser == null) throw new KeyNotFoundException($"User with ID {user.IdUser} not found.");
            existingUser.UserName = user.UserName;
            existingUser.Phone = user.Phone;
            await _db.SaveChangesAsync();
        }

      
        public async Task DeleteUserAsync(int id)
        {
            var existingUser = await _db.Users.FindAsync(id);
            if (existingUser == null) throw new KeyNotFoundException($"User with ID {id} not found.");
            _db.Users.Remove(existingUser);
            await _db.SaveChangesAsync();
        }
    }
}
