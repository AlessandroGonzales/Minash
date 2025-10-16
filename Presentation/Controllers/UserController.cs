using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.DTO.Request;
using Application.DTO.Partial;
using Application.Authentication;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _userService;
        private readonly IRoleAppService _roleService;
        private readonly JwtService _jwtService;
        public UserController(
            IUserAppService service,
            IRoleAppService roleService,
            JwtService jwtService
        )
        {
            _userService = service;
            _roleService = roleService;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("by-city")]
        public async Task<IActionResult> GetUsersByCityAsync([FromQuery] string city)
        {
            var users = await _userService.GetUsersByCityAsync(city);
            return Ok(users);
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> GetUserByNameAsync([FromQuery] string name)
        {
            var user = await _userService.GetUserByNameAsync(name);
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("by-Role/{roleId}")]
        public async Task<IActionResult> GetUsersByRoleId(int roleId)
        {
            var users = await _userService.GetUsersByRolIdAsync(roleId);
            return Ok(users);
        }

        [HttpGet("by_email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest userDto)
        {
            var createdUser = await _userService.AddUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.IdUser }, createdUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login ([FromBody] LoginRequest login)
        {
            var user = await _userService.ValidateUserAsync(login.Email, login.Password);
            if (user == null)
                return Unauthorized("Credenciales invalidas");
            var rol = await _roleService.GetRoleByIdAsync(user.IdRol);
            var token = _jwtService.GenerateToken(user, rol);
            return Ok(new { Token = token});
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute]int id, [FromBody] UserRequest userDto)
        {
            await _userService.UpdateUserAsync(id, userDto);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserNameAndPhoneAsync([FromRoute]int id, [FromBody] UserPartial useDto)
        {
            await _userService.PartialUpdateUserAsync(id, useDto);
            return NoContent();
        }
     
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
