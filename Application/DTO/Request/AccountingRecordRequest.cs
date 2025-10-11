using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class AccountingRecordRequest
    {
        public int IdAccountingRecord { get; set; }
        [Required]
        [Range(1, (double)decimal.MaxValue, ErrorMessage = "El Total debe ser mayor a 0" )]
        public decimal total {  get; set; }
        [Required]
        public string? Details { get; set; }
        [Required]
        public int idPay { get; set; }
    }
}
