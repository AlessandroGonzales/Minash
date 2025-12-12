namespace Application.DTO.Response
{
    public class GarmentServiceResponse
    {
        public int IdGarmentService { get; set; }
        public int IdGarments { get; set; }
        public int IdService { get; set; }
        public decimal AddtionalPrice { get; set; }
        public List<string> ImageUrl { get; set; } = null!;
        public List<string> Colors { get; set; } = null!;
        public List<string> Sizes { get; set; } = null!;
        public string GarmentServiceName { get; set; } = null!;
        public string GarmentServiceDetails { get; set; } = null!;
    }
}
