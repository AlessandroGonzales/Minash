using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.DTO.Partial;
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

        [HttpGet]
        public async Task<IActionResult> GetAllGarments()
        {
            var garments = await _service.GetAllGarmentsAsync();
            return Ok(garments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGarmentById(int id)
        {
            var garment = await _service.GetGarmentByIdAsync(id);
            if (garment == null)
                return NotFound();
            return Ok(garment);
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetGarmentByName([FromQuery] string name)
        {
            var garments = await _service.GetGarmentsByNameAsync(name);
            return Ok(garments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGarment([FromBody] GarmentRequest garmentDto)
        {
            var createdGarment = await _service.AddGarmentAsync(garmentDto);
            return CreatedAtAction(nameof(GetGarmentById), new { id = createdGarment.IdGarment }, createdGarment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGarment([FromRoute]int id, [FromBody] GarmentRequest garmentDto)
        {
            await _service.UpdateGarmentAsync(id, garmentDto);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateGarmentAsync([FromRoute]int id, [FromBody] GarmentPartial garmentDto)
        {
            await _service.PartialUpdateGarmentAsync(id, garmentDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGarment(int id)
        {
            await _service.DeleteGarmentAsync(id);
            return NoContent();
        }
    }
}
