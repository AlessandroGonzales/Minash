using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoController  : ControllerBase
    {
        private readonly IVideoAppService _videoAppService;
        private readonly IWebHostEnvironment _environment;
        public VideoController(IVideoAppService videoAppService, IWebHostEnvironment environment)
        {
            _videoAppService = videoAppService;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> AddVideo([FromForm] VideosRequest videoRequest)
        {
            await _videoAppService.AddVideoAsync(videoRequest, _environment.WebRootPath);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetVideos()
        {
            var videos = await _videoAppService.GetVideosAsync();
            return Ok(videos);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteVideo([FromQuery] int id)
        {
            await _videoAppService.DeleteVideoAsync(id);
            return NoContent();
        }
    }
}
