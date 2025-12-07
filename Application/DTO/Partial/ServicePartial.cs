using Microsoft.AspNetCore.Http;

namespace Application.DTO.Partial
{
    public class ServicePartial
    {
        public decimal ServicePrice { get; set; }
        public IFormFile ImageUrl { get; set; } = null!;   
    }
}
