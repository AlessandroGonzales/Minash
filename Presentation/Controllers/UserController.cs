using Application.Authentication;
using Application.DTO.Partial;
using Application.DTO.Request;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _userService;
        private readonly IRoleAppService _roleService;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public UserController(
            IUserAppService service,
            IRoleAppService roleService,
            JwtService jwtService,
            IConfiguration configuration,
            IWebHostEnvironment env
        )
        {
            _userService = service;
            _roleService = roleService;
            _jwtService = jwtService;
            _configuration = configuration;
            _env = env;
        }

        [Authorize(Policy = "CEOOrAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("by-city")]
        public async Task<IActionResult> GetUsersByCityAsync([FromQuery] string city)
        {
            var users = await _userService.GetUsersByCityAsync(city);
            return Ok(users);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("by-name")]
        public async Task<IActionResult> GetUserByNameAsync([FromQuery] string name)
        {
            var user = await _userService.GetUserByNameAsync(name);
            return Ok(user);
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }


        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            // Extraemos el ID del claim "nameidentifier" (estándar en ASP.NET)
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.net/ws/2005/05/identity/claims/nameidentifier")
                              ?? User.FindFirst("nameid")
                              ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                return Unauthorized("No se pudo identificar al usuario.");
            }

            var user = await _userService.GetUserByIdAsync(currentUserId);
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Ok(user);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("by-Role/{roleId}")]
        public async Task<IActionResult> GetUsersByRoleId(int roleId)
        {
            var users = await _userService.GetUsersByRolIdAsync(roleId);
            return Ok(users);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("by_email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] UserRequest userDto)
        {
            var createdUser = await _userService.AddUserAsync(userDto, _env.WebRootPath);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.IdUser }, createdUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var user = await _userService.ValidateUserAsync(login.Email, login.Password);
            if (user == null)
                return Unauthorized("Credenciales invalidas");
            var rol = await _roleService.GetRoleByIdAsync(user.IdRol);

            var token = _jwtService.GenerateToken(user, rol);
            return Ok(new { Token = token });
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserRequest userDto)
        {
            await _userService.UpdateUserAsync(id, userDto);
            return NoContent();
        }

        [Authorize(Policy = "ClienteOrAdmin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateUserAsync([FromRoute] int id, [FromForm] UserPartial useDto)
        {
            await _userService.PartialUpdateUserAsync(id, useDto, _env.WebRootPath);
            return NoContent();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

    }
}
