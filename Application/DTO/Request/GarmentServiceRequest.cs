using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class GarmentServiceRequest
    {
        public int IdGarmentService { get; set; }
        [Required]
        [Range(1, (double)decimal.MaxValue, ErrorMessage = "El rango debe ser mayor a 0")]
        public decimal AdditionalPrice { get; set; }
        public List<IFormFile> ImageFiles { get; set; } = null!;
        [Required]
        public int IdGarment { get; set; }
        public List<string> Colors { get; set; } = new List<string>();
        public List<string> Sizes { get; set; } = new List<string>();
        [Required]
        public int IdService { get; set; }
        public string GarmentServiceName { get; set; } = null!;
        public string GarmentServiceDetails { get; set; } = null!;
    }
}
