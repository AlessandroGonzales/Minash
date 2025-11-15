using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class CustomAppService : ICustomAppService
    {
        private readonly ICustomRepository _repo;
        private readonly IOrderRepository _orderRepo;
        public CustomAppService(ICustomRepository repo, IOrderRepository orderRepo)
        {
            _repo = repo;
            _orderRepo = orderRepo;
        }



        private static CustomResponse MapToResponse(Custom custom) => new CustomResponse
        {
            IdCustom = custom.IdCustom,
            IdService = custom.IdService,
            IdGarment = custom.IdGarment,
            IdUser = custom.IdUser,
            CustomDetails = custom.CustomerDetails,
            Count = custom.Count,
            ImageUrl = custom.ImageUrl,
        };

        private static Custom MapToDomain(CustomRequest dto) => new Custom
        {
            IdCustom = dto.IdCustom,
            IdService = dto.IdService,
            IdGarment = dto.IdGarment,
            IdUser = dto.IdUser,
            CustomerDetails = dto.CustomerDetails,
            Count = dto.Count,
            ImageUrl = dto.ImageUrl,
        };

        private static Custom MapToDomain(CustomPartial dto) => new Custom
        {
            ImageUrl = dto.ImageUrl,
            Count = dto.count,
        };

        public async Task<IEnumerable<CustomResponse>> GetAllCustomsAsync()
        {
            var list = await _repo.GetAllCustomsAsync();
            return list.Select(MapToResponse);
        }

        public async Task<IEnumerable<CustomResponse>> GetCustomsByUserNameAsync(string userName)
        {
            var list = await _repo.GetCustomsByUserNameAsync(userName);
            return list.Select(MapToResponse);
        }

        public async Task<CustomResponse> GetCustomByIdAsync(int id)
        {

            var custom = await _repo.GetCustomByIdAsync(id);
            if (custom == null)
            {
                throw new KeyNotFoundException($"Custom with ID {id} not found.");
            }
            return MapToResponse(custom);
        }

        public async Task<CustomResponse> AddCustomAsync(CustomRequest custom)
        {
            var creatCustom = MapToDomain(custom);
            var createdCustom = await _repo.AddCustomAsync(creatCustom);

            var order = new Order
            {
                IdCustom = createdCustom.IdCustom,
                IdUser = createdCustom.IdUser,
                Total = createdCustom.Service.Price, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _orderRepo.AddCustomOrderAsync(order);
            return MapToResponse(createdCustom);
        }

        public async Task UpdateCustomAsync(int id, CustomRequest custom)
        {
            var UpdateCustom = MapToDomain(custom);
            await _repo.UpdateCustomAsync(id, UpdateCustom);
        }

        public async Task PartialUpdateCustomAsync(int id, CustomPartial custom)
        {
            var UpdateCustom = MapToDomain(custom);
            await _repo.PartialUpdateCustomAsync(id, UpdateCustom);
        }

        public async Task DeleteCustomAsync(int id)
        {
            await _repo.DeleteCustomAsync(id);
        }
    }
}
