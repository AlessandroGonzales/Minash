using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class GarmentRequest
    {
        public int IdGarment { get; set; }

        [Required(ErrorMessage = "El nombre de la prenda es obligatorio")]
        public string GarmentName { get; set; } = null!;

        [Required(ErrorMessage = "Los detalles son obligatorios")]
        public string GarmentDetails { get; set; } = null!;
        public IFormFile ImageUrl { get; set; } = null!;
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
        public decimal Price { get; set; }
        public List<string>? Colors { get; set; }
        public List<string>? Sizes { get; set; }
    }
}
