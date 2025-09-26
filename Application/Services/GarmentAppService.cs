using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class GarmentAppService : IGarmentAppService
    {
        private readonly IGarmentRepository _repo;
        public GarmentAppService(IGarmentRepository repo)
        {
            _repo = repo;
        }

        private static GarmentDto MaptoDto(Garment d) => new GarmentDto
        {
            IdGarment = d.IdGarment,
            GarmentName = d.GarmentName,
            GarmentDetails = d.GarmentDetails,
            ImageUrl = d.ImageUrl,
        };

        private static Garment MaptoDomain(GarmentDto dto) => new Garment
        {
            IdGarment = dto.IdGarment,
            GarmentName = dto.GarmentName,
            GarmentDetails = dto.GarmentDetails,
            ImageUrl = dto.ImageUrl,
        };

        public async Task<IEnumerable<GarmentDto>> GetAllGarmentsAsync()
        {
            var list = await _repo.GetAllGarmentsAsync();
            return list.Select(MaptoDto);
        }

        public async Task<GarmentDto?> GetGarmentByIdAsync(int id)
        { 
            var garment = await _repo.GetGarmentByIdAsync(id);
            return garment == null ? null : MaptoDto(garment);
        }

        public async Task<IEnumerable<GarmentDto>> GetGarmentsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<GarmentDto>();
            var normalizedName = name.Replace(" ", "").ToLower();
            var list = await _repo.GetGarmentsByNameAsync(normalizedName);
            return list.Select(MaptoDto);
        }

        public async Task<GarmentDto> AddGarmentAsync(GarmentDto garment)
        {
            var domainGarment = MaptoDomain(garment);
            var createdGarment = await _repo.AddGarmentAsync(domainGarment);
            return MaptoDto(createdGarment);
        }

        public async Task UpdateGarmentAsync(GarmentDto garment)
        {
            var domainGarment = MaptoDomain(garment);
            await _repo.UpdateGarmentAsync(domainGarment);
        }
        public async Task DeleteGarmentAsync(int id)
        {
            await _repo.DeleteGarmentAsync(id);
        }
    }
}
