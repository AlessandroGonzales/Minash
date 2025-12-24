namespace Application.DTO.Response
{
    public class DetailsOrderResponse
    {
        public int IdDetailsOrder { get; set; }
        public int IdOrder { get; set; }
        public int? IdGarmentService { get; set; }
        public int? IdService { get; set; }
        public string? selectColor { get; set; }
        public string? selectSize { get; set; }
        public string? details { get ; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }

    }
}
