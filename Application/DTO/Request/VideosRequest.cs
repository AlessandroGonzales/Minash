using Microsoft.AspNetCore.Http;

namespace Application.DTO.Request
{
    public class VideosRequest
    {
        public int Id { get; set; }

        public IFormFile Url { get; set; } = null!;
        public string type { get; set; } = null!;

    }
}
