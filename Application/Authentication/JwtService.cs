using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Application.DTO.Response;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Application.Authentication
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<JwtService> _logger; 
        public JwtService(IConfiguration config, ILogger<JwtService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public string GenerateToken(UserResponse user, RoleResponse role)
        {
    
            var jwtSettings = _config.GetSection("JwtSettings");
            _logger?.LogInformation("=== JWT Generate: User={UserId}, Role={Role}", user.IdUser, role.RoleName);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // Debug
            if (string.IsNullOrEmpty(jwtSettings["Key"])) throw new InvalidOperationException("JWT Key missing!");
            var duration = double.Parse(jwtSettings["DurationInMinutes"] ?? "60");  // Fallback

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role.RoleName),
                new Claim("imageUrl", user.ImageUrl ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"])),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
