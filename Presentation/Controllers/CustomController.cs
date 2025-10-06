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
            return CreatedAtAction(nameof(GetCustomByIdAsync),new { Id = custom.IdCustom }, custom);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCustomAsync([FromBody] CustomRequest domain)
        {
            await _service.UpdateCustomAsync(domain);
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
