namespace Application.DTO.Response
{
    public class ServiceResponse
    {
        public int IdService { get; set; }
        public string ServiceName { get; set; } = null!;
        public string ServiceDetails { get; set; } = null!;
        public decimal ServicePrice { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
