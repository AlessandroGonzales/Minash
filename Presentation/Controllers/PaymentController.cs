using Application.DTO.Partial;
using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentAppService _service;
        public PaymentController(IPaymentAppService service) { _service = service; }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAllPaymentsAsync()
        {
            var list = await _service.GetAllPaymentsAsync();
            return Ok(list);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentByIdAsync([FromRoute] int id)
        {
            var pay = await _service.GetPaymentByIdAsync(id);
            return Ok(pay);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("by-idOrder/{id}")]
        public async Task<IActionResult> GetPaymentByIdOrder([FromRoute] int id)
        {
            var pay = await _service.GetPaymentsByOrderIdAsync(id);
            return Ok(pay);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddPaymentAsync([FromBody] PaymentRequest payment)
        {
            var createdPay = await _service.AddPaymentAsync(payment);
            return CreatedAtAction(nameof(GetPaymentByIdAsync), new { Id = createdPay.IdPay }, createdPay);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentAsync([FromRoute]int id, [FromBody] PaymentRequest payment)
        {
            await _service.UpdatePaymentAsync(id,payment);
            return NoContent();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdatePaymentAsync([FromRoute] int id, [FromBody] PaymentPartial payment)
        {
            await _service.PartialUpdatePaymentAsync(id, payment); 
            return NoContent();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentAsync(int id)
        {
            await _service.DeletePaymentAsync(id);
            return NoContent();
        }

        [HttpPost("test-mercadopago")]
        [Authorize(Policy = "ClienteOrAdmin")]
        public async Task<IActionResult> TestMercadoPagoAsync([FromBody] PaymentRequest payment)
        {
            var result = await _service.AddPaymentAsync(payment);
            return Ok(result);
        }

    }
}
