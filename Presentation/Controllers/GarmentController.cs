using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.DTO.Request;
using Application.DTO.Partial;
using Microsoft.AspNetCore.Authorization;
namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GarmentController  : ControllerBase
    {
        private readonly IGarmentAppService _service;
        public GarmentController(IGarmentAppService service)
        {
            _service = service;
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAllGarments()
        {
            var garments = await _service.GetAllGarmentsAsync();
            return Ok(garments);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGarmentById(int id)
        {
            var garment = await _service.GetGarmentByIdAsync(id);
            if (garment == null)
                return NotFound();
            return Ok(garment);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("name")]
        public async Task<IActionResult> GetGarmentByName([FromQuery] string name)
        {
            var garments = await _service.GetGarmentsByNameAsync(name);
            return Ok(garments);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateGarment([FromBody] GarmentRequest garmentDto)
        {
            var createdGarment = await _service.AddGarmentAsync(garmentDto);
            return CreatedAtAction(nameof(GetGarmentById), new { id = createdGarment.IdGarment }, createdGarment);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGarment([FromRoute]int id, [FromBody] GarmentRequest garmentDto)
        {
            await _service.UpdateGarmentAsync(id, garmentDto);
            return NoContent();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateGarmentAsync([FromRoute]int id, [FromBody] GarmentPartial garmentDto)
        {
            await _service.PartialUpdateGarmentAsync(id, garmentDto);
            return NoContent();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGarment(int id)
        {
            await _service.DeleteGarmentAsync(id);
            return NoContent();
        }
    }
}
