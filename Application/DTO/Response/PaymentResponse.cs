namespace Application.DTO.Response
{
    public class PaymentResponse
    {
        public int IdPay { get; set; }
        public int IdOrder { get; set; }
        public decimal Total { get; set; }
    }

}
