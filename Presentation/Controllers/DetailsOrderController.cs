using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetailsOrderController : ControllerBase
    {
        private readonly IDetailsOrderAppService _service;
        public DetailsOrderController(IDetailsOrderAppService service)
        {
            _service = service;
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAllDetailsOrders()
        {
            var detailsOrders = await _service.GetAllDetailsOrdersAsync();
            return Ok(detailsOrders);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailsOrderById(int id)
        {
            var detailsOrder = await _service.GetDetailsOrderByIdAsync(id);
            if (detailsOrder == null)
                return NotFound();
            return Ok(detailsOrder);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("by-order/{orderId}")]
        public async Task<IActionResult> GetDetailsOrdersByOrderId(int orderId)
        {
            var detailsOrders = await _service.GetDetailsOrdersByOrderIdAsync(orderId);
            return Ok(detailsOrders);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateDetailsOrder([FromBody] DetailsOrderRequest detailsOrderDto)
        {
            var createdDetailsOrder = await _service.AddDetailsOrderAsync(detailsOrderDto);
            return CreatedAtAction(nameof(GetDetailsOrderById), new { id = createdDetailsOrder.IdDetailsOrder }, createdDetailsOrder);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetailsOrder([FromRoute]int id, [FromBody] DetailsOrderRequest detailsOrderDto)
        {
            await _service.UpdateDetailsOrderAsync(id, detailsOrderDto);
            return NoContent();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetailsOrder(int id)
        {
            await _service.DeleteDetailsOrderAsync(id);
            return NoContent();
        }
    }
}
