using Microsoft.AspNetCore.Http;

namespace Application.DTO.Partial
{
    public class UserPartial
    {
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public string? Email { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? FullAddress { get; set; }

    }
}
