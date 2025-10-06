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
        public CustomAppService(ICustomRepository repo) { _repo = repo; }


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

        public async Task<IEnumerable<CustomResponse>> GetAllCustomsAsync()
        {
            var list = await _repo.GetAllCustomsAsync();
            return list.Select(MapToResponse);
        }

        public async Task<CustomResponse> GetCustomByIdAsync(int id)
        {
            var custom = await _repo.GetCustomByIdAsync(id);
            return MapToResponse(custom);
        }

        public async Task<CustomResponse> AddCustomAsync(CustomRequest custom)
        {
            var creatCustom = MapToDomain(custom);
            var createdCustom = await _repo.AddCustomAsync(creatCustom);
            return MapToResponse(createdCustom);
        }

        public async Task UpdateCustomAsync(CustomRequest custom)
        {
            var UpdateCustom = MapToDomain(custom);
            await _repo.UpdateCustomAsync(UpdateCustom);
        }

        public async Task DeleteCustomAsync(int id)
        {
            await _repo.DeleteCustomAsync(id);
        }
    }
}
