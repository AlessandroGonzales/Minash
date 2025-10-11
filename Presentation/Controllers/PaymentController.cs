using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentAppService _service;
        public PaymentController(IPaymentAppService service) { _service = service; }

        [HttpGet]
        public async Task<IActionResult> GetAllPaymentsAsync()
        {
            var list = await _service.GetAllPaymentsAsync();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetPaymentByIdAsync([FromRoute] int id)
        {
            var pay = await _service.GetPaymentByIdAsync(id);
            return Ok(pay);
        }

        [HttpGet("by-IdOrder/{Id}")]
        public async Task<IActionResult> GetPaymentByIdOrder([FromRoute] int id)
        {
            var pay = await _service.GetPaymentsByOrderIdAsync(id);
            return Ok(pay);
        }
        [HttpPost]
        public async Task<IActionResult> AddPaymentAsync([FromBody] PaymentRequest payment)
        {
            var createdPay = await _service.AddPaymentAsync(payment);
            return CreatedAtAction(nameof(GetPaymentByIdAsync), new { Id = createdPay.IdPay }, createdPay);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentAsync([FromRoute]int id, [FromBody] PaymentRequest payment)
        {
            await _service.UpdatePaymentAsync(id,payment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentAsync(int Id)
        {
            await _service.DeletePaymentAsync(Id);
            return NoContent();
        }

    }
}
