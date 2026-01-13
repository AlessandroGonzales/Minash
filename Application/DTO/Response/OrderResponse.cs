using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.DTO.Response
{
    public class OrderResponse
    {
        public int IdOrder{ get; set; }
        public int IdUser{ get; set; }
        public decimal Total{ get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderState State{ get; set; }

    }
}
