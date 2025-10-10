﻿using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class GarmentServiceAppService : IGarmentServiceAppService
    {
        private readonly IGarmentServiceRepository _repo;
        public GarmentServiceAppService(IGarmentServiceRepository repo)
        {
            _repo = repo;
        }

        private static GarmentServiceResponse MapToResponse(GarmentService d) => new GarmentServiceResponse
        {
            IdGarmentService = d.IdGarmentService,
            IdGarments = d.IdGarment,
            IdService = d.IdService,
            AddtionalPrice = d.AdditionalPrice,
            ImageUrl = d.ImageUrl,
        };

        private static GarmentService MapToDomain(GarmentServiceRequest dto) 
        {

            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new GarmentService
            {
                IdGarmentService = dto.IdGarmentService,
                IdGarment = dto.IdGarment,
                IdService = dto.IdService,
                AdditionalPrice = dto.AdditionalPrice,
                ImageUrl = dto.ImageUrl,
            };
        }

        private static GarmentService MapToDomain(GarmentServicePartial dto) => new GarmentService
        {
            AdditionalPrice = dto.AdditionalPrice,
            ImageUrl = dto.ImageUrl,
        };

        public async Task<IEnumerable<GarmentServiceResponse>> GetAllGarmentServicesAsync()
        {
            var list = await _repo.GetAllGarmentServicesAsync();
            return list.Select(MapToResponse);
        }

        public async Task<GarmentServiceResponse?> GetGarmentServiceByIdAsync(int id)
        {
            var domain = await _repo.GetGarmentServiceByIdAsync(id);
            return domain != null ? MapToResponse(domain) : null;
        }

        public async Task<IEnumerable<GarmentServiceResponse>> GetGarmentServicesByGarmentIdAsync(int garmentId)
        {
            var domainList = await _repo.GetGarmentsServiceByGarmentIdAsync(garmentId); 
            return domainList.Select(MapToResponse);
        }

        public async Task<IEnumerable<GarmentServiceResponse>> GetGarmentServicesByServiceIdAsync(int serviceId)
        {
            var domainList = await _repo.GetGarmentsServiceByServiceIdAsync(serviceId);
            return domainList.Select(MapToResponse);
        }

        public async Task<IEnumerable<GarmentServiceResponse>> GetGarmentServicesByQualityAsync(string quality)
        {
            IEnumerable<GarmentService> garmentServices = quality switch
            {
                "Premium" => await _repo.GetGarmentServicesByPriceAsync(500, 1000),
                "Standard" => await _repo.GetGarmentServicesByPriceAsync(100, 499),
                "Basic" => await _repo.GetGarmentServicesByPriceAsync(1, 99),
                _ => Enumerable.Empty<GarmentService>(),
            };

            return garmentServices.Select(MapToResponse);
        }
        public async Task<GarmentServiceResponse> AddGarmentServiceAsync(GarmentServiceRequest dto)
        {
            if (dto.IdGarment <= 0 || dto.IdService <= 0)
                throw new ArgumentException("IDs de Garment y Service deben ser mayores a 0.");

            var domain = MapToDomain(dto);
            domain.CreatedAt = DateTime.UtcNow; 
            domain.UpdatedAt = DateTime.UtcNow;

            var addedDomain = await _repo.AddGarmentServiceAsync(domain);
            return MapToResponse(addedDomain);
        }

        public async Task UpdateGarmentServiceAsync(int id, GarmentServiceRequest dto)
        {
            if (dto.IdGarmentService <= 0)
                throw new ArgumentException("ID de GarmentService debe ser mayor a 0.");

            var domain = MapToDomain(dto);
            domain.UpdatedAt = DateTime.UtcNow; 

            await _repo.UpdateGarmentServiceAsync(id, domain);
        }

        public async Task PartialUpdateGarmentServiceAsync(int id, GarmentServicePartial dto)
        {
           if(id <= 0)
                throw new ArgumentException("ID de GarmentService debe ser mayor a 0.");

            var domain = MapToDomain(dto);
            await _repo.PartialUpdateGarmentServiceAsync(id, domain);
        }

        public async Task DeleteGarmentServiceAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0.");

            await _repo.DeleteGarmentServiceAsync(id);
        }
    }
}
