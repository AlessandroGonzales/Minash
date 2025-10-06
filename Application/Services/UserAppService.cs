using Application.DTO.Request;
using Application.DTO.Response;
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
        private static UserResponse MapToResponse(User d) => new UserResponse
        {
            IdUser = d.IdUser,
            UserName = d.UserName,
            LastName = d.LastName,
            Email = d.Email,
            Address = d.Address,
            PhoneNumber = d.Phone,
            ImageUrl = d.ImageUrl,
            Province = d.Province,
            City = d.City,
            FullAddress = d.FullAddress,
            IdRol = d.IdRole,
        };
        private static User MapToDomain(UserRequest dto) 
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
        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var list = await _repo.GetAllUsersAsync();
            return list.Select(MapToResponse);
        }

        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            var domain = await _repo.GetUserByIdAsync(id);
            return domain != null ? MapToResponse(domain) : null;
        }
        public async Task<UserResponse?> GetUserByEmailAsync(string email)
        {
            var domain = await _repo.GetUserByEmailAsync(email);
            return domain != null ? MapToResponse(domain) : null;
        }
        public async Task<IEnumerable<UserResponse>> GetUsersByRolIdAsync(int roleId)
        {
            var domainList = await _repo.GetUsersByRolIdAsync(roleId); 
            return domainList.Select(MapToResponse);
        }
        public async Task<UserResponse>AddUserAsync(UserRequest user)
        {
            if(user.IdRole <= 0)
                throw new ArgumentException("Role ID must be greater than zero.");

            var domain = MapToDomain(user);
            var addedDomain = await _repo.AddUserAsync(domain);
            return MapToResponse(addedDomain);
        }

        public async Task UpdateUserAsync(UserRequest user)
        {
            var domain = MapToDomain(user);

            await _repo.UpdateUserAsync(domain);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _repo.DeleteUserAsync(id);
        }
    }
}
