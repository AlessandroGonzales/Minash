using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTO.Request;
using Application.DTO.Partial;
using Microsoft.AspNetCore.Authorization;


namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase 
    {
        private readonly IServiceAppService _service;
        private readonly IWebHostEnvironment _env;
        public ServiceController(IServiceAppService service, IWebHostEnvironment env    )
        {
            _service = service;
            _env = env;
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _service.GetAllServicesAsync();
            return Ok(services);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById([FromRoute]int id)
        {
            var service = await _service.GetServiceByIdAsync(id);
            if (service == null)
                return NotFound();
            return Ok(service);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("name")]
        public async Task<IActionResult> GetServiceByName([FromQuery] string name)
        {
            var services = await _service.GetServiceByNameAsync(name);
            return Ok(services);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("filter")]
        public async Task<IActionResult> GetServicesByqualityAsync([FromQuery] string quality)
        {
            var list = await _service.GetServicesByQualityAsync(quality);
            return Ok(list);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateService([FromForm] ServiceRequest serviceDto)
        {
            var createdService = await _service.AddServiceAsync(serviceDto, _env.WebRootPath);
            return CreatedAtAction(nameof(GetServiceById), new { id = createdService.IdService }, createdService);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService([FromRoute]int id, [FromBody] ServiceRequest serviceDto)
        {
            await _service.UpdateServiceAsync(id, serviceDto);
            return NoContent();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateServicePriceAsync([FromRoute] int id, [FromBody] ServicePartial serviceDto)
        {
            await _service.PartialUpdateServiceAsync(id, serviceDto);
            return NoContent();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            await _service.DeleteServiceAsync(id);
            return NoContent();
        }   
    }
}
