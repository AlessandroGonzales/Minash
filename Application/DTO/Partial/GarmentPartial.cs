using Microsoft.AspNetCore.Http;

namespace Application.DTO.Partial
{
    public class GarmentPartial
    {
        public string GarmentDetails { get; set; } = null!;
        public IFormFile ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
