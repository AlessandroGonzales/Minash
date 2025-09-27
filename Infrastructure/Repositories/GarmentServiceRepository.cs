using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EfGarmentService = Infrastructure.Persistence.Entities.GarmentService;
using EfGarment = Infrastructure.Persistence.Entities.Garment;
using EfService = Infrastructure.Persistence.Entities.Service;

namespace Infrastructure.Repositories
{
    public class GarmentServiceRepository : IGarmentServiceRepository
    {
        private readonly MinashDbContext _db;

        public GarmentServiceRepository(MinashDbContext db)
        {
            _db = db;
        }

        // Helper para mapear Garment (extrae para reutilizar)
        private static Garment MapToDomainGarment(EfGarment efGarment)
        {
            if (efGarment == null) return null!; // O lanza excepción si requerido

            return new Garment
            {
                IdGarment = efGarment.IdGarment,
                GarmentName = efGarment.GarmentName,
                GarmentDetails = efGarment.GarmentDetails,
                ImageUrl = efGarment.ImageUrl ?? string.Empty,
                CreatedAt = efGarment.CreatedAt ?? DateTime.UtcNow, // Usa UtcNow como fallback
                UpdatedAt = efGarment.UpdatedAt ?? DateTime.UtcNow,
            };
        }

        // Helper para mapear Service (similar)
        private static Service MapToDomainService(EfService efService)
        {
            if (efService == null) return null!;

            return new Service
            {
                IdService = efService.IdService,
                ServiceName = efService.ServiceName,
                ServiceDetails = efService.ServiceDetails,
                Price = efService.Price,
                ImageUrl = efService.ImageUrl ?? string.Empty,
                CreatedAt = efService.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = efService.UpdatedAt ?? DateTime.UtcNow,
            };
        }

        private static GarmentService MapToDomain(EfGarmentService ef)
        {
            return new GarmentService
            {
                IdGarmentService = ef.IdGarmentService,
                AdditionalPrice = ef.AdditionalPrice,
                ImageUrl = ef.ImageUrl ?? string.Empty,
                CreatedAt = ef.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = ef.UpdatedAt ?? DateTime.UtcNow,
                IdGarment = ef.IdGarment,
                Garment = MapToDomainGarment(ef.IdGarmentNavigation),
                IdService = ef.IdService,
                Service = MapToDomainService(ef.IdServiceNavigation),
            };
        }

        private static EfGarmentService MapToEf(GarmentService domain)
        {
            return new EfGarmentService
            {
                IdGarmentService = domain.IdGarmentService, // Para updates
                AdditionalPrice = domain.AdditionalPrice,
                ImageUrl = domain.ImageUrl,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
                IdGarment = domain.IdGarment,
                IdService = domain.IdService,
            };
        }

        // Método helper para query base con Includes
        private IQueryable<EfGarmentService> GetQueryableWithIncludes(bool tracking = false)
        {
            var query = _db.GarmentServices
                .Include(gs => gs.IdGarmentNavigation)
                .Include(gs => gs.IdServiceNavigation);

            return tracking ? query : query.AsNoTracking();
        }

        public async Task<IEnumerable<GarmentService>> GetAllGarmentServicesAsync()
        {
            var list = await GetQueryableWithIncludes().ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<GarmentService?> GetGarmentServiceByIdAsync(int id)
        {
            var ef = await GetQueryableWithIncludes().FirstOrDefaultAsync(gs => gs.IdGarmentService == id);
            return ef == null ? null : MapToDomain(ef);
        }

        public async Task<IEnumerable<GarmentService>> GetGarmentsServiceByGarmentIdAsync(int garmentId) // Corregí el typo
        {
            var list = await GetQueryableWithIncludes()
                .Where(gs => gs.IdGarment == garmentId)
                .ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<IEnumerable<GarmentService>> GetGarmentsServiceByServiceIdAsync(int serviceId)
        {
            var list = await GetQueryableWithIncludes()
                .Where(gs => gs.IdService == serviceId)
                .ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<GarmentService> AddGarmentServiceAsync(GarmentService garmentService)
        {
            // Valida FKs en dominio si no lo haces ya
            if (garmentService.IdGarment <= 0 || garmentService.IdService <= 0)
                throw new ArgumentException("IDs de Garment y Service deben ser válidos.");

            var ef = MapToEf(garmentService);
            _db.GarmentServices.Add(ef);
            await _db.SaveChangesAsync();

            // Recarga con Includes para poblar navegaciones
            var addedEf = await GetQueryableWithIncludes().FirstAsync(gs => gs.IdGarmentService == ef.IdGarmentService);
            return MapToDomain(addedEf);
        }

        public async Task UpdateGarmentServiceAsync(GarmentService garmentService)
        {
            var ef = await _db.GarmentServices.FindAsync(garmentService.IdGarmentService);
            if (ef == null) throw new KeyNotFoundException($"GarmentService con ID {garmentService.IdGarmentService} no encontrado.");

            // Mapea solo campos actualizables (no navegaciones, ya que EF las maneja por FK)
            ef.AdditionalPrice = garmentService.AdditionalPrice;
            ef.ImageUrl = garmentService.ImageUrl;
            ef.UpdatedAt = DateTime.UtcNow; // Actualiza timestamp
            ef.IdGarment = garmentService.IdGarment; // Si cambias FK, EF migra la relación
            ef.IdService = garmentService.IdService;

            await _db.SaveChangesAsync();
            // Nota: No recargo aquí porque Update no devuelve la entidad en tu interfaz.
            // Si lo necesitas, agrega un return como en Add.
        }

        public async Task DeleteGarmentServiceAsync(int id)
        {
            var ef = await _db.GarmentServices.FindAsync(id);
            if (ef == null) throw new KeyNotFoundException($"GarmentService con ID {id} no encontrado.");

            _db.GarmentServices.Remove(ef);
            await _db.SaveChangesAsync();
        }
    }
}