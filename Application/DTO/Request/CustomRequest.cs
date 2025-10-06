namespace Application.DTO.Request
{
    public class CustomRequest
    {
        public int IdCustom { get; set; }
        public string CustomerDetails { get; set; } = null!;
        public int Count { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int IdUser { get; set; }
        public int IdGarment { get; set; }
        public int IdService { get; set; }
    }
}
