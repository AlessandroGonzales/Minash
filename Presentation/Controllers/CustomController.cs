using Application.DTO.Partial;
using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomController : ControllerBase
    {
        private readonly ICustomAppService _service;
        private readonly IWebHostEnvironment _env;
        public CustomController(ICustomAppService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [Authorize(Policy = "CEOOrAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAllCustomAsync()
        {
            var list = await _service.GetAllCustomsAsync();
            return Ok(list);
        }

        [Authorize(Policy = "CEOOrAdmin")]
        [HttpGet("by-userName")]
        public async Task<IActionResult> GetCustomsByUserNameAsync([FromQuery] string userName)
        {
            var list = await _service.GetCustomsByUserNameAsync(userName);
            return Ok(list);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomByIdAsync([FromRoute] int id)
        {
            var custom = await _service.GetCustomByIdAsync(id);
            return Ok(custom);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddCustomAsync([FromForm] CustomRequest domain)
        {
            var custom = await _service.AddCustomAsync(domain, _env.ContentRootPath); 
            return CreatedAtAction(nameof(GetCustomByIdAsync), new { id  = custom.IdCustom }, custom);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomAsync([FromRoute] int id, [FromBody] CustomRequest domain)
        {
            await _service.UpdateCustomAsync(id, domain);
            return NoContent();
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateCustomAsync([FromRoute] int id, [FromBody] CustomPartial domain)
        {
            await _service.PartialUpdateCustomAsync(id, domain);
            return NoContent();
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomAsync([FromRoute] int id)
        {
            await _service.DeleteCustomAsync(id);
            return NoContent();
        }

    }
}
