using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _repo;
        public UserAppService(IUserRepository repo)
        {
            _repo = repo;
        }
        private static UserDto MapToDto(User d) => new UserDto
        {
            IdUser = d.IdUser,
            UserName = d.UserName,
            LastName = d.LastName,
            Email = d.Email,
            Phone = d.Phone,
            Address = d.Address,
            ImageUrl = d.ImageUrl,
            Province = d.Province,
            City = d.City,
            FullAddress = d.FullAddress,
            PasswordHash = d.PasswordHash,
            IdRole = d.IdRole,
        };
        private static User MapToDomain(UserDto dto) 
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new User
            {
                IdUser = dto.IdUser,
                UserName = dto.UserName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                ImageUrl = dto.ImageUrl,
                Province = dto.Province,
                City = dto.City,
                FullAddress = dto.FullAddress,
                PasswordHash = dto.PasswordHash,
                IdRole = dto.IdRole,
            };
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var list = await _repo.GetAllUsersAsync();
            return list.Select(MapToDto);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var domain = await _repo.GetUserByIdAsync(id);
            return domain != null ? MapToDto(domain) : null;
        }
        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var domain = await _repo.GetUserByEmailAsync(email);
            return domain != null ? MapToDto(domain) : null;
        }
        public async Task<IEnumerable<UserDto>> GetUsersByRolIdAsync(int roleId)
        {
            var domainList = await _repo.GetUsersByRolIdAsync(roleId); 
            return domainList.Select(MapToDto);
        }
        public async Task<UserDto> AddUserAsync(UserDto user)
        {
            if(user.IdRole <= 0)
                throw new ArgumentException("Role ID must be greater than zero.");

            var domain = MapToDomain(user);
            domain.CreatedAt = DateTime.UtcNow;
            domain.UpdatedAt = DateTime.UtcNow;
            var addedDomain = await _repo.AddUserAsync(domain);
            return MapToDto(addedDomain);
        }

        public async Task UpdateUserAsync(UserDto user)
        {
            var domain = MapToDomain(user);
            domain.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateUserAsync(domain);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _repo.DeleteUserAsync(id);
        }
    }
}
