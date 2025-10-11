using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class ServiceRequest
    {
        public int IdService { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string ServiceName { get; set; } = null!;

        [Required]
        public string ServiceDetails { get; set; } = null!;

        [Required]
        [Range(1, (double)decimal.MaxValue, ErrorMessage = "El rango debe ser mayor a 0")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = null!;
    }
}

