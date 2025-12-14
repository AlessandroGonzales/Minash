using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EfGarment = Infrastructure.Persistence.Entities.Garment;
namespace Infrastructure.Repositories
{
    public class GarmentRepository : IGarmentRepository
    {
        private readonly MinashDbContext _db;
        public GarmentRepository(MinashDbContext db)
        {
            _db = db;
        }

        private static Garment MapToDomain(EfGarment efGarment) => new Garment
        {
            IdGarment = efGarment.IdGarment,
            GarmentName = efGarment.GarmentName,
            GarmentDetails = efGarment.GarmentDetails,
            ImageUrl = efGarment.ImageUrl ?? string.Empty,
            Sizes = efGarment.Sizes ?? new List<string>(),
            Colors = efGarment.Colors ?? new List<string>(),
            UpdatedAt = efGarment.UpdatedAt ?? DateTime.UtcNow,
            CreatedAt = efGarment.CreatedAt ?? DateTime.UtcNow,
            Price = efGarment.Price,

            GarmentServices = efGarment.GarmentServices.Select(gs => new GarmentService
            {
                IdGarmentService = gs.IdGarmentService,
                AdditionalPrice = gs.AdditionalPrice,
                Sizes = gs.Sizes,
                Colors = gs.Colors,
                GarmentServiceName = gs.GarmentServiceName,
                GarmentServiceDetails = gs.GarmentServiceDetails,
                ImageUrl = gs.ImageUrl,
                CreatedAt = gs.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = gs.UpdatedAt ?? DateTime.UtcNow,
                IdGarment = gs.IdGarment,
                IdService = gs.IdService
            }).ToList(),

            Customs = efGarment.Customs.Select(c => new Custom
            {
                IdCustom = c.IdCustom,
                CustomerDetails = c.CustomerDetails,
                Count = c.Count,
                ImageUrl = c.ImageUrl ?? new List<string>(),
                CreatedAt = c.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = c.UpdatedAt ?? DateTime.UtcNow,
                IdGarment = c.IdGarment,
                IdUser = c.IdUser,
                IdService = c.IdService
            }).ToList()
        };

        private static EfGarment MapToEf(Garment d) => new EfGarment
        {
            GarmentName = d.GarmentName,
            GarmentDetails = d.GarmentDetails,
            Price = d.Price,
            ImageUrl = d.ImageUrl,
            UpdatedAt = d.UpdatedAt,
            CreatedAt = d.CreatedAt,
            Sizes = d.Sizes,
            Colors = d.Colors,
        };

        public async Task<IEnumerable<Garment>> GetAllGarmentsAsync()
        {
            var list = await _db.Garments.AsNoTracking().ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<Garment?> GetGarmentByIdAsync(int id)
        {
            var efGarment = await _db.Garments.FindAsync(id);
            return efGarment == null ? null : MapToDomain(efGarment);
        }

        public async Task<IEnumerable<Garment>> GetGarmentsByNameAsync(string name)
        {
            var list = await _db.Garments
                .Where(g => g.GarmentName.Replace(" ", "").ToLower().Contains(name))
                .AsNoTracking()
                .ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<Garment> AddGarmentAsync(Garment garment)
        {
            var efGarment = MapToEf(garment);
            _db.Garments.Add(efGarment);
            await _db.SaveChangesAsync();
            garment.IdGarment = efGarment.IdGarment;
            return garment;
        }

        public async Task UpdateGarmentAsync(int id, Garment garment)
        {
            var efGarment = await _db.Garments.FindAsync(id);
            if (efGarment == null) throw new KeyNotFoundException($"Garment with ID {garment.IdGarment} not found.");

            efGarment.GarmentName = garment.GarmentName;
            efGarment.GarmentDetails = garment.GarmentDetails;
            efGarment.ImageUrl = garment.ImageUrl;
            efGarment.Price = garment.Price;
            efGarment.UpdatedAt = DateTime.UtcNow;

            _db.Garments.Update(efGarment);
            await _db.SaveChangesAsync();
        }

        public async Task PartialUpdateGarmentAsync(int id, Garment garment)
        {
            var efGarment = await _db.Garments.FindAsync(id);
            if (efGarment == null) throw new KeyNotFoundException($"Garment with ID {id} not found.");

            if (garment.ImageUrl is not null)
                efGarment.ImageUrl = garment.ImageUrl;

            if(garment.GarmentDetails is not null)
                efGarment.GarmentDetails = garment.GarmentDetails;

            if(garment.Price != 0)
                efGarment.Price = garment.Price;

            _db.Garments.Update(efGarment);
            await _db.SaveChangesAsync();

        }
        public async Task DeleteGarmentAsync(int id)
        {
            var efGarment = await _db.Garments.FindAsync(id);
            if (efGarment == null) throw new KeyNotFoundException($"Garment with ID {id} not found.");
            _db.Garments.Remove(efGarment);
            await _db.SaveChangesAsync();
        }
    }
}
