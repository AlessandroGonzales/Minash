using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class GarmentServiceRequest
    {
        public int IdGarmentService { get; set; }
        [Required]
        [Range(1, (double)decimal.MaxValue, ErrorMessage = "El rango debe ser mayor a 0")]
        public decimal AdditionalPrice { get; set; }
        public string ImageUrl { get; set; } = null!;
        [Required]
        public int IdGarment { get; set; }
        [Required]
        public int IdService { get; set; }
    }
}
