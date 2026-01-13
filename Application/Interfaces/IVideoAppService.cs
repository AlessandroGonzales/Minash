using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IVideoAppService
    {
        Task<IEnumerable<VideoResponse>> GetVideosAsync();
        Task AddVideoAsync(VideosRequest video, string webRootPath);
        Task DeleteVideoAsync(int id);
    }
}
