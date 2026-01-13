using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.DTO.Partial
{
    public class OrderPartial
    {
        public decimal Total { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderState State  { get; set; }
    }
}
