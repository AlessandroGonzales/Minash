using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTO.Request;
using Application.DTO.Response;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ServiceController : ControllerBase 
    {
        private readonly IServiceAppService _service;
        public ServiceController(IServiceAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _service.GetAllServicesAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await _service.GetServiceByIdAsync(id);
            if (service == null)
                return NotFound();
            return Ok(service);
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetServiceByName([FromQuery] string name)
        {
            var services = await _service.GetServiceByNameAsync(name);
            return Ok(services);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetServicesByqualityAsync([FromQuery] string quality)
        {
            var list = await _service.GetServicesByQualityAsync(quality);
            return Ok(list);
        }
            

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServiceRequest serviceDto)
        {
            var createdService = await _service.AddServiceAsync(serviceDto);
            return CreatedAtAction(nameof(GetServiceById), new { id = createdService.IdService }, createdService);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateService([FromBody] ServiceRequest serviceDto)
        {
            await _service.UpdateServiceAsync(serviceDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            await _service.DeleteServiceAsync(id);
            return NoContent();
        }   

    }
}
