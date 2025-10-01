using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class CustomAppService : ICustomAppService
    {
        private readonly ICustomRepository _repo;
        public CustomAppService(ICustomRepository repo) { _repo = repo; }


        private static CustomDto MapToDto(Custom custom) => new CustomDto
        {
            IdCustom = custom.IdCustom,
            IdService = custom.IdService,
            IdGarment = custom.IdGarment,
            IdUser = custom.IdUser,
            CustomerDetails = custom.CustomerDetails,
            Count = custom.Count,
            ImageUrl = custom.ImageUrl,
        };

        private static Custom MapToDomain(CustomDto dto) => new Custom
        {
            IdCustom = dto.IdCustom,
            IdService = dto.IdService,
            IdGarment = dto.IdGarment,
            IdUser = dto.IdUser,
            CustomerDetails = dto.CustomerDetails,
            Count = dto.Count,
            ImageUrl = dto.ImageUrl,
        };

        public async Task<IEnumerable<CustomDto>> GetAllCustomsAsync()
        {
            var list = await _repo.GetAllCustomsAsync();
            return list.Select(MapToDto);
        }

        public async Task<CustomDto> GetCustomByIdAsync(int id)
        {
            var custom = await _repo.GetCustomByIdAsync(id);
            return MapToDto(custom);
        }

        public async Task<CustomDto> AddCustomAsync(CustomDto custom)
        {
            var creatCustom = MapToDomain(custom);
            var createdCustom = await _repo.AddCustomAsync(creatCustom);
            return MapToDto(createdCustom);
        }

        public async Task UpdateCustomAsync(CustomDto custom)
        {
            var UpdateCustom = MapToDomain(custom);
            UpdateCustom.UpdatedAt = DateTime.Now;

            await _repo.UpdateCustomAsync(UpdateCustom);
        }

        public async Task DeleteCustomAsync(int id)
        {
            await _repo.DeleteCustomAsync(id);
        }
    }
}
