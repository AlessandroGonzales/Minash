using Microsoft.AspNetCore.Http;

namespace Application.DTO.Partial
{
    public class GarmentServicePartial
    {
        public decimal AdditionalPrice { get; set; }
        public IFormFile? ImageFile { get; set; }

    }
}
