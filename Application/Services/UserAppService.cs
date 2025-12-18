using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using BCrypt.Net;

namespace Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _repo;
        private readonly IFileStorageService _fileStorage;
        public UserAppService(IUserRepository repo, IFileStorageService fileStorage)
        {
            _repo = repo;
            _fileStorage = fileStorage;
        }
        private static UserResponse MapToResponse(User d) => new UserResponse
        {
            IdUser = d.IdUser,
            UserName = d.UserName,
            LastName = d.LastName,
            Email = d.Email,
            Address = d.Address,
            PhoneNumber = d.Phone,
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
                Province = dto.Province,
                City = dto.City,
                FullAddress = dto.FullAddress,
                PasswordHash = dto.PasswordHash,
                IdRole = dto.IdRole,
            };
        }
        private static User MapToDomain(UserPartial dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new User
            {
                UserName = dto.UserName,
                Phone = dto.Phone,
            };
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var list = await _repo.GetAllUsersAsync();
            return list.Select(MapToResponse);
        }

        public async Task<IEnumerable<UserResponse>> GetUsersByRolIdAsync(int roleId)
        {
            var domainList = await _repo.GetUsersByRolIdAsync(roleId); 
            return domainList.Select(MapToResponse);
        }

        public async Task<IEnumerable<UserResponse>> GetUsersByCityAsync(string city)
        {
            var list = await _repo.GetUsersByCityAsync(city);
            return list.Select(MapToResponse);
        }

        public async Task <UserResponse?> GetUserByNameAsync(string name)
        {
            var user = await _repo.GetUserByNameAsync(name);
            return user == null ? null : MapToResponse(user);
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
        public async Task<UserResponse>AddUserAsync(UserRequest user, string webRootPath)
        {
            if(user.IdRole <= 0)
                throw new ArgumentException("Role ID must be greater than zero.");
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.IdRole = 1;
            string imageUrl = null;
            if ( user.ImageUrl != null)
            {
                imageUrl = await _fileStorage.UploadFileAsync(user.ImageUrl, "users", webRootPath);
            }

            var domain = MapToDomain(user);
            domain.ImageUrl = imageUrl;
            var addedDomain = await _repo.AddUserAsync(domain);
            return MapToResponse(addedDomain);
        }

        public async Task UpdateUserAsync(int id, UserRequest user)
        {
            var domain = MapToDomain(user);

            await _repo.UpdateUserAsync(id, domain);
        }

        public async Task PartialUpdateUserAsync(int id, UserPartial user)
        {
            var domain = MapToDomain(user);

            await _repo.PartialUpdateUserAsync(id, domain);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _repo.DeleteUserAsync(id);
        }

        public async Task<UserResponse?> ValidateUserAsync(string email, string password)
        {
            var user = await _repo.GetUserByEmailAsync(email);
            if (user == null) return null;

            bool passwordIsValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!passwordIsValid) return null;

            return MapToResponse(user);
        }
    }
}
