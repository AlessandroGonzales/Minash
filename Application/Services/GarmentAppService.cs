using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;
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

        private static GarmentResponse MapToResponse(Garment d) => new GarmentResponse
        {
            IdGarment = d.IdGarment,
            GarmentName = d.GarmentName,
            GarmentDetails = d.GarmentDetails,
            ImageUrl = d.ImageUrl,
        };

        private static Garment MaptoDomain(GarmentRequest dto) => new Garment
        {
            IdGarment = dto.IdGarment,
            GarmentName = dto.GarmentName,
            GarmentDetails = dto.GarmentDetails,
            ImageUrl = dto.ImageUrl,
        };

        private static Garment MapToDomain(GarmentPartial dto) => new Garment
        {
            GarmentDetails = dto.GarmentDetails,
            ImageUrl = dto.ImageUrl
        };

        public async Task<IEnumerable<GarmentResponse>> GetAllGarmentsAsync()
        {
            var list = await _repo.GetAllGarmentsAsync();
            return list.Select(MapToResponse);
        }

        public async Task<GarmentResponse?> GetGarmentByIdAsync(int id)
        { 
            var garment = await _repo.GetGarmentByIdAsync(id);
            return garment == null ? null : MapToResponse(garment);
        }

        public async Task<IEnumerable<GarmentResponse>> GetGarmentsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<GarmentResponse>();
            var normalizedName = name.Replace(" ", "").ToLower();
            var list = await _repo.GetGarmentsByNameAsync(normalizedName);
            return list.Select(MapToResponse);
        }

        public async Task<GarmentResponse> AddGarmentAsync(GarmentRequest garment)
        {
            var domainGarment = MaptoDomain(garment);
            var createdGarment = await _repo.AddGarmentAsync(domainGarment);
            return MapToResponse(createdGarment);
        }

        public async Task UpdateGarmentAsync(int id, GarmentRequest garment)
        {
            var domainGarment = MaptoDomain(garment);
            await _repo.UpdateGarmentAsync(id, domainGarment);
        }

        public async Task PartialUpdateGarmentAsync(int id, GarmentPartial garment)
        {
            var domainGarment = MapToDomain(garment);

            await _repo.PartialUpdateGarmentAsync(id, domainGarment);
        }
        public async Task DeleteGarmentAsync(int id)
        {
            await _repo.DeleteGarmentAsync(id);
        }
    }
}
