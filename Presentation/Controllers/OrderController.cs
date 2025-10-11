using Application.DTO.Partial;
using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderAppService _service;
        public OrderController(IOrderAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _service.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _service.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }
        [HttpGet("by-userName")]
        public async Task<IActionResult> GetOrdersByUserNameAsync([FromQuery] string userName)
        {
            var orders = await _service.GetOrdersByUserNameAsync(userName);
            return Ok(orders);
        }
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId([FromRoute] int userId)
        {
            var orders = await _service.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderDto)
        {
            var createdOrder = await _service.AddOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.IdOrder }, createdOrder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute]int id, [FromBody] OrderRequest orderDto)
        {
            await _service.UpdateOrderAsync(id, orderDto);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateOrderAsync([FromRoute] int id, [FromBody] OrderPartial orderDto)
        {
            await _service.PartialUpdateOrderAsync(id, orderDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _service.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
