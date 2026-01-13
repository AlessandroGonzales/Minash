namespace Application.DTO.Response
{
    public class CustomResponse
    {
        public int IdCustom { get; set; }
        public int IdUser { get; set; }
        public int? IdGarment { get; set; }
        public string? SelectedColor { get; set; }
        public string? SelectedSize { get; set; }
        public int? IdService { get; set; }
        public int? IdGarmentService { get; set; }
        public List<string> ImageUrl { get; set; } = null!;
        public string CustomerDetails { get; set; } = null!;
        public string CustomerName { get; set; }
        public int Count { get; set; }
        public int IdOrder { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? CustomTotal { get; set; }
    }
}
