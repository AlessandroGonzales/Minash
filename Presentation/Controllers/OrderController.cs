using Application.DTO.Partial;
using Application.DTO.Request;
using Application.Interfaces;
using Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderAppService _service;
        private readonly GmailClient _gmailClient;
        public OrderController(IOrderAppService service, GmailClient gmailClient)
        {
            _service = service;
            _gmailClient = gmailClient;
        }

        [Authorize(Policy = "CEOPolicy")]
        [HttpGet("paid-orders")]
        public async Task<IActionResult> GetAllPaidOrders()
        {
            var orders = await _service.GetAllPaidOrdersAsync();
            return Ok(orders);
        }


        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _service.GetAllOrdersAsync();
            return Ok(orders);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _service.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("by-userName")]
        public async Task<IActionResult> GetOrdersByUserNameAsync([FromQuery] string userName)
        {
            var orders = await _service.GetOrdersByUserNameAsync(userName);
            return Ok(orders);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId([FromRoute] int userId)
        {
            var orders = await _service.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("draft")]
        public async Task<IActionResult> GetDraftOrderByUserId()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var order = await _service.GetDraftOrderByUserIdAsync(userId);
            return Ok(order);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("paid")]
        public async Task<IActionResult> GetPaidOrderByUserId()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var order = await _service.GetPaidOrderByUserIdAsync(userId);
            return Ok(order);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpPost("butget")]
        public async Task<IActionResult> createbutget([FromBody] BudgetRequestDto budgetRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _gmailClient.SendBudgetAlertAsync(
                    budgetRequest.Name,
                    budgetRequest.Email,
                    budgetRequest.Phone,
                    budgetRequest.Category,
                    budgetRequest.Description
                );

                return Ok(new { message = "Solicitud enviada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al enviar el correo", details = ex.Message });
            }
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderDto)
        {
            var createdOrder = await _service.AddOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.IdOrder }, createdOrder);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute]int id, [FromBody] OrderRequest orderDto)
        {
            await _service.UpdateOrderAsync(id, orderDto);
            return NoContent();
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateOrderAsync([FromRoute] int id, [FromBody] OrderPartial orderDto)
        {
            await _service.PartialUpdateOrderAsync(id, orderDto);
            return NoContent();
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _service.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
