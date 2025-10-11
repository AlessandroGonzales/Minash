using Application.DTO.Partial;
using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomController : ControllerBase
    {
        private readonly ICustomAppService _service;
        public CustomController(ICustomAppService service) { _service = service; }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomAsync()
        {
            var list = await _service.GetAllCustomsAsync();
            return Ok(list);
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> GetCustomsByUserNameAsync([FromQuery] string userName)
        {
            var list = await _service.GetCustomsByUserNameAsync(userName);
            return Ok(list);
        }
            
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomByIdAsync([FromRoute] int id)
        {
            var custom = await _service.GetCustomByIdAsync(id);
            return Ok(custom);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomAsync([FromBody] CustomRequest domain)
        {
            var custom = await _service.AddCustomAsync(domain); 
            return CreatedAtAction(nameof(GetCustomByIdAsync), new { id  = custom.IdCustom }, custom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomAsync([FromRoute] int id, [FromBody] CustomRequest domain)
        {
            await _service.UpdateCustomAsync(id, domain);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateCustomAsync([FromRoute] int id, [FromBody] CustomPartial domain)
        {
            await _service.PartialUpdateCustomAsync(id, domain);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomAsync([FromRoute] int id)
        {
            await _service.DeleteCustomAsync(id);
            return NoContent();
        }

    }
}
