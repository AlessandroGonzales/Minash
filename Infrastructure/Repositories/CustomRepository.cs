using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EfCustom = Infrastructure.Persistence.Entities.Custom;
using EfGarment = Infrastructure.Persistence.Entities.Garment;
using EfService = Infrastructure.Persistence.Entities.Service;
using EfUser = Infrastructure.Persistence.Entities.User;

namespace Infrastructure.Repositories
{
    public class CustomRepository : ICustomRepository
    {
        private readonly MinashDbContext _db;
        public CustomRepository(MinashDbContext db) { _db = db; }

        private static User MapToDomainUser(EfUser user) => new User
        {
            IdUser = user.IdUser,
            UserName = user.UserName,
            Address = user.Address,
            FullAddress = user.FullAddress,
            City = user.City,
            Email = user.Email,
            Phone = user.Phone,
            Province = user.Province,
            ImageUrl = user.ImageUrl,
            CreatedAt = user.CreatedAt ?? DateTime.UtcNow,
            LastName = user.LastName,
            PasswordHash = user.PasswordHash,
            UpdatedAt = user.UpdatedAt ?? DateTime.UtcNow,
            IdRole = user.IdRole,

        };

        private static Garment MapToDomainGarment(EfGarment garment) => new Garment
        {
            IdGarment = garment.IdGarment,
            GarmentDetails = garment.GarmentDetails,
            ImageUrl = garment.ImageUrl,
            GarmentName = garment.GarmentName,
            CreatedAt = garment.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = garment.UpdatedAt ?? DateTime.UtcNow,
        };

        private static Service MapToDomainService(EfService service) => new Service
        {
            IdService = service.IdService,
            ServiceName = service.ServiceName,
            ServiceDetails = service.ServiceDetails,
            ImageUrl = service.ImageUrl,
            CreatedAt = service.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt= service.UpdatedAt ?? DateTime.UtcNow,
            Price = service.Price,
        };

        private static Custom MapToDomain(EfCustom custom) => new Custom
        {
            IdCustom = custom.IdCustom,
            Count = custom.Count,
            ImageUrl = custom.ImageUrl,
            CreatedAt= custom.CreatedAt ?? DateTime.UtcNow,
            CustomerDetails = custom.CustomerDetails,
            UpdatedAt= custom.UpdatedAt ?? DateTime.UtcNow,
            IdService = custom.IdService,
            Service = MapToDomainService(custom.IdServiceNavigation),
            IdUser = custom.IdUser,
            User = MapToDomainUser(custom.IdUserNavigation),
            IdGarment = custom.IdGarment,
            Garment = MapToDomainGarment(custom.IdGarmentNavigation),
        };

        private static EfCustom MapToEf(Custom custom) => new EfCustom
        {
            IdCustom = custom.IdCustom,
            Count = custom.Count,
            ImageUrl = custom.ImageUrl,
            CustomerDetails = custom.CustomerDetails,
            IdService = custom.IdService,
            IdUser = custom.IdUser,
            IdGarment = custom.IdGarment,
        };

        private IQueryable<EfCustom> GetQueryableWithIncludes(bool tracking = false)
        {
            var list = _db.Customs
                .Include(x => x.IdUserNavigation)
                .Include(x => x.IdServiceNavigation)
                .Include(x => x.IdGarmentNavigation);

            return tracking ? list : list.AsNoTracking();
        }

        public async Task<IEnumerable<Custom>> GetCustomsByUserNameAsync(string userName)
        {
            var list = await GetQueryableWithIncludes()
                .Where(o => o.IdUserNavigation.UserName == userName)
                .ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<IEnumerable<Custom>> GetAllCustomsAsync()
        {
            var list = await GetQueryableWithIncludes().ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task <Custom?> GetCustomByIdAsync(int id)
        {
            var custom = await GetQueryableWithIncludes().FirstOrDefaultAsync(s => s.IdCustom == id);
            return custom == null ? null : MapToDomain(custom);
        }

        public async Task <Custom> AddCustomAsync (Custom custom)
        {
            var creatCustom = MapToEf(custom);
            _db.Customs.Add(creatCustom);
            await _db.SaveChangesAsync();
            var createdCustom = await GetQueryableWithIncludes().FirstAsync(s => s.IdCustom == creatCustom.IdCustom);
            return MapToDomain(createdCustom);
        }

        public async Task UpdateCustomAsync (int id, Custom custom)
        {
            var rCustom = await _db.Customs.FindAsync(id);
            rCustom.UpdatedAt = DateTime.UtcNow;
            rCustom.CustomerDetails = custom.CustomerDetails;
            rCustom.Count = custom.Count;
            rCustom.ImageUrl = custom.ImageUrl;

            _db.Customs.Update(rCustom);
            await _db.SaveChangesAsync();
        }

        public async Task PartialUpdateCustomAsync(int id, Custom custom)
        {
            var rCustom = await _db.Customs.FindAsync(id);

            if (custom.ImageUrl is not null)
                rCustom.ImageUrl = custom.ImageUrl;

            if (custom.Count == 0) rCustom.Count = rCustom.Count;
            else rCustom.Count = custom.Count;

            _db.Customs.Update(rCustom);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteCustomAsync(int id)
        {
            var rCustom = await _db.Customs.FindAsync(id);
            _db.Customs.Remove(rCustom);
            await _db.SaveChangesAsync();
        }
    }
}
