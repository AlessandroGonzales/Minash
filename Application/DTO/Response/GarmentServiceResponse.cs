namespace Application.DTO.Response
{
    public class GarmentServiceResponse
    {
        public int IdGarmentService { get; set; }
        public int IdGarments { get; set; }
        public int IdService { get; set; }
        public decimal AddtionalPrice { get; set; }
        public string ImageUrl { get; set; } = null!;

    }
}
