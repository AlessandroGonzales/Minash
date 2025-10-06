using Application.DTO.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GarmentServiceController : ControllerBase
    {
        private readonly IGarmentServiceAppService _service;
        public GarmentServiceController(IGarmentServiceAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGarmentServices()
        {
            var garmentServices = await _service.GetAllGarmentServicesAsync();
            return Ok(garmentServices);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetGarmentServiceById(int id)
        {
            var garmentService = await _service.GetGarmentServiceByIdAsync(id);
            if (garmentService == null)
                return NotFound();
            return Ok(garmentService);
        }
        [HttpGet("by-garment/{garmentId}")]
        public async Task<IActionResult> GetGarmentServicesByGarmentId(int garmentId)
        {
            var garmentServices = await _service.GetGarmentServicesByGarmentIdAsync(garmentId);
            return Ok(garmentServices);
        }
        [HttpPost]
        public async Task<IActionResult> CreateGarmentService([FromBody] GarmentServiceRequest garmentServiceDto)
        {
            var createdGarmentService = await _service.AddGarmentServiceAsync(garmentServiceDto);
            return CreatedAtAction(nameof(GetGarmentServiceById), new { id = createdGarmentService.IdGarmentService }, createdGarmentService);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateGarmentService([FromBody] GarmentServiceRequest garmentServiceDto)
        {
            await _service.UpdateGarmentServiceAsync(garmentServiceDto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGarmentService(int id)
        {
            await _service.DeleteGarmentServiceAsync(id);
            return NoContent();
        }

    }
}
