using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class DetailsOrderRequest
    {
        public int IdDetailsOrder { get; set; }

        [Required]
        [Range(1, (double)decimal.MaxValue, ErrorMessage ="La cantidad debe ser mayor a 0")]
        public int Count { get; set; }
        [Required]
        [Range(1, (double)decimal.MaxValue, ErrorMessage ="El subtotal debe ser mayor a 0")]
        public decimal Subtotal { get; set; }
        [Required]
        [Range(1,(double)decimal.MaxValue, ErrorMessage ="El precio por unidad debe ser mayor a 0")]
        public decimal UnitPrice { get; set; }
        [Required]
        public int IdOrder { get; set; }
        [Required]
        public int IdGarmentService { get; set; }
    }
}
