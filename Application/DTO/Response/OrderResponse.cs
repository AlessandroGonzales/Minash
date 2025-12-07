namespace Application.DTO.Response
{
    public class OrderResponse
    {
        public int IdOrder{ get; set; }
        public int IdUser{ get; set; }
        public int? IdCustom{ get; set; }
        public decimal TotalPrice{ get; set; }

    }
}
