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
        public string CustomDetails { get; set; } = null!;
        public int Count { get; set; }
    }
}
