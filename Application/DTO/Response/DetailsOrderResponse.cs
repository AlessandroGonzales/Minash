namespace Application.DTO.Response
{
    public class DetailsOrderResponse
    {
        public int IdDetailsOrder { get; set; }
        public int IdOrder { get; set; }
        public int? IdGarmentService { get; set; }
        public int? IdService { get; set; }
        public string? SelectColor { get; set; }
        public string? SelectSize { get; set; }
        public string? Details { get ; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public string ServiceName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;

    }
}
