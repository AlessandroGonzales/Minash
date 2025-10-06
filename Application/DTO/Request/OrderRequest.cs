namespace Application.DTO.Request
{
    public class OrderRequest
    {
        public int IdOrder { get; set; }
        public decimal Total { get; set; }
        public int IdUser { get; set; }
    }
}
