using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTO.Request
{
    public class OrderRequest
    {
        public int IdOrder { get; set; }
        [Required]
        [Range(1, (double)decimal.MaxValue, ErrorMessage ="El total debe ser mayor a 0")]
        public decimal Total { get; set; }
        [Required]
        public int IdUser { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderState State { get; set; }
    }
}
