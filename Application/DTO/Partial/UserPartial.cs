using Microsoft.AspNetCore.Http;

namespace Application.DTO.Partial
{
    public class UserPartial
    {
        public string UserName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public IFormFile ImageUrl { get; set; } = null!;
    }
}
