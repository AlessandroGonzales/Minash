using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
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
            UpdatedAt = efGarment.UpdatedAt.HasValue ? DateTime.SpecifyKind(efGarment.UpdatedAt.Value, DateTimeKind.Utc) : DateTime.MinValue,
            CreatedAt = efGarment.CreatedAt.HasValue ? DateTime.SpecifyKind(efGarment.CreatedAt.Value, DateTimeKind.Utc) : DateTime.MinValue,
        };

        private static EfGarment MapToEf(Garment d) => new EfGarment
        {
            GarmentName = d.GarmentName,
            GarmentDetails = d.GarmentDetails,
            ImageUrl = d.ImageUrl,
            UpdatedAt = d.UpdatedAt,
            CreatedAt = d.CreatedAt,
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

        public async Task UpdateGarmentAsync(Garment garment)
        {
            var efGarment = await _db.Garments.FindAsync(garment.IdGarment);
            if (efGarment == null) throw new KeyNotFoundException($"Garment with ID {garment.IdGarment} not found.");
            efGarment.GarmentName = garment.GarmentName;
            efGarment.GarmentDetails = garment.GarmentDetails;
            efGarment.ImageUrl = garment.ImageUrl;
            efGarment.UpdatedAt = DateTime.UtcNow;
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
