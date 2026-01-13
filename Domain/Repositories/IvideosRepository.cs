using Domain.Entities;

namespace Domain.Repositories
{
    public interface IVideosRepository
    {
        Task<IEnumerable<Video>> GetVideos();
        Task AddAsync(Video video);
        Task DeleteAsync(int id);
    }
}
