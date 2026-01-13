using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class VideosRepository : IVideosRepository
    {
        private readonly MinashDbContext _context;

        public VideosRepository(MinashDbContext context)
        {
            _context = context;
        }

        private static Video MapToDomainVideo(Persistence.Entities.Video efVideo)
        {
            if (efVideo == null) return null!;
            return new Video
            {
                Id = efVideo.Id,
                Url = efVideo.Url,
                type = efVideo.Type,
                CreatedAt = DateTime.UtcNow,
            };
        }

        private static Persistence.Entities.Video MapToEfVideo(Video video)
        {
            if (video == null) return null!;
            return new Persistence.Entities.Video
            {
                Id = video.Id,
                Url = video.Url,
                Type = video.type,
                CreatedAt = video.CreatedAt,
            };
        }
        public async Task<IEnumerable<Video>> GetVideos()
        {
            var list = await _context.Videos.AsNoTracking().ToListAsync();
            return list.Select(MapToDomainVideo);
        }

        public async Task AddAsync(Video video)
        {
            var createdVideo = MapToEfVideo(video);
            await _context.Videos.AddAsync(createdVideo);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var videoToDelete = await _context.Videos.FindAsync(id);
            if (videoToDelete != null)
            {
                _context.Videos.Remove(videoToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
