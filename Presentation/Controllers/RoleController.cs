using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleAppService _service;
        public RoleController(IRoleAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _service.GetAllRolesAsync();
            return Ok(roles);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _service.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();
            return Ok(role);
        }
        [HttpGet("name")]
        public async Task<IActionResult> GetRoleByName([FromQuery] string name)
        {
            var roles = await _service.GetRolesByNameAsync(name);
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
        {
            var createdRole = await _service.AddRoleAsync(roleDto);
            return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.IdRole }, createdRole);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDto roleDto)
        {
            await _service.UpdateRoleAsync(roleDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _service.DeleteRoleAsync(id);
            return NoContent();
        }


    }
}
