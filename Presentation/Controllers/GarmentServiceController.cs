using Application.DTO.Partial;
using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GarmentServiceController : ControllerBase
    {
        private readonly IGarmentServiceAppService _service;
        private readonly IWebHostEnvironment _env;
        public GarmentServiceController(IGarmentServiceAppService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAllGarmentServices()
        {
            var garmentServices = await _service.GetAllGarmentServicesAsync();
            return Ok(garmentServices);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGarmentServiceById(int id)
        {
            var garmentService = await _service.GetGarmentServiceByIdAsync(id);
            if (garmentService == null)
                return NotFound();
            return Ok(garmentService);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("by-garment/{garmentId}")]
        public async Task<IActionResult> GetGarmentServicesByGarmentId(int garmentId)
        {
            var garmentServices = await _service.GetGarmentServicesByGarmentIdAsync(garmentId);
            return Ok(garmentServices);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("By-service/{serviceId}")]
        public async Task<IActionResult> GetGarmentServicesByServiceIdAsync(int serviceId)
        {
            var garmentServices = await _service.GetGarmentServicesByServiceIdAsync(serviceId);
            return Ok(garmentServices);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("filter")]
        public async Task<IActionResult> GetGarmentServicesByQualityAsync(string  quality)
        {
            var garmentServies = await _service.GetGarmentServicesByQualityAsync(quality);
            return Ok(garmentServies);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateGarmentService([FromForm] GarmentServiceRequest garmentServiceDto)
        {
            var createdGarmentService = await _service.AddGarmentServiceAsync(garmentServiceDto, _env.WebRootPath);
            return CreatedAtAction(nameof(GetGarmentServiceById), new { id = createdGarmentService.IdGarmentService }, createdGarmentService);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGarmentService([FromRoute]int id, [FromBody] GarmentServiceRequest garmentServiceDto)
        {
            await _service.UpdateGarmentServiceAsync(id, garmentServiceDto);
            return NoContent();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateGarmentServcieAsync([FromRoute] int id, [FromBody] GarmentServicePartial garmentServiceDto)
        {
            await _service.PartialUpdateGarmentServiceAsync(id, garmentServiceDto);
            return NoContent();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGarmentService(int id)
        {
            await _service.DeleteGarmentServiceAsync(id);
            return NoContent();
        }

    }
}
