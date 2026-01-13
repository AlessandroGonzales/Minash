using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class VideoAppService : IVideoAppService
    {
        private readonly IVideosRepository _videosRepository;
        private readonly IFileStorageService _fileStorageService;

        public VideoAppService(IVideosRepository videosRepository, IFileStorageService fileStorageService)
        {
            _videosRepository = videosRepository;
            _fileStorageService = fileStorageService;
        }

        private static Video MapToDomainVideo(VideosRequest video)
        {
            if (video == null) return null!;
            return new Video
            {
                Id =video.Id,
                type = video.type,
            };
        }

        private static VideoResponse MapToDomainVideoResponse(Video video)
        {
            if (video == null) return null!;
            return new VideoResponse
            {
                Id = video.Id,
                Url = video.Url,
                type = video.type,
            };
        }

        public async Task AddVideoAsync(VideosRequest video, string webRootPath)
        {
            
            string videoDto = null;

            if (video.Url != null)
            {
                videoDto = await _fileStorageService.UploadFileAsync(video.Url, "Videos", webRootPath);
            }

            var createdVideo = MapToDomainVideo(video);
            createdVideo.Url = videoDto;
            await _videosRepository.AddAsync(createdVideo);

        }
        public async Task<IEnumerable<VideoResponse>> GetVideosAsync()
        {
            var listVideos = await _videosRepository.GetVideos();
            return listVideos.Select(MapToDomainVideoResponse);
        }

        public async Task DeleteVideoAsync(int id)
        {
            await _videosRepository.DeleteAsync(id);
        }
    }
}
