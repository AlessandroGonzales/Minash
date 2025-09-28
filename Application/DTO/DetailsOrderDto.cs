namespace Application.DTO
{
    public class DetailsOrderDto
    {
        public int IdDetailsOrder { get; set; }
        public int Count { get; set; }
        public decimal Subtotal { get; set; }
        public decimal UnitPrice { get; set; }
        public int IdOrder { get; set; }
        public int IdGarmentService { get; set; }
    }
}
