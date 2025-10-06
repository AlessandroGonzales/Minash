using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.DTO.Request;
namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _service;
        public UserController(IUserAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _service.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("by-Role/{roleId}")]
        public async Task<IActionResult> GetUsersByRoleId(int roleId)
        {
            var users = await _service.GetUsersByRolIdAsync(roleId);
            return Ok(users);
        }

        [HttpGet("by_email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _service.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest userDto)
        {
            var createdUser = await _service.AddUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.IdUser }, createdUser);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserRequest userDto)
        {
            await _service.UpdateUserAsync(userDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _service.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
