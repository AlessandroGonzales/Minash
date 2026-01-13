using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class DetailsOrderRequest
    {
        public int IdDetailsOrder { get; set; }

        [Required]
        [Range(1, (double)decimal.MaxValue, ErrorMessage ="La cantidad debe ser mayor a 0")]
        public int Count { get; set; }
        public decimal Subtotal { get; set; }
        public decimal UnitPrice { get; set; }
        public string? SelectedColor { get; set; }
        public string? Details { get; set; }
        public string? SelectedSize { get; set; }
        public int? IdGarmentService { get; set; }
        public int? IdService { get; set; }
    }
}
