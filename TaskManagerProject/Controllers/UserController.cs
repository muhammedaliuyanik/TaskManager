using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using TaskManagerProject.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace TaskManagerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;

        public UserController(DatabaseContext context, IConfiguration configuration, ILogger<UserController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
{
    try
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }

        var token = GenerateJwtToken(user);
        _logger.LogInformation("Generated JWT token for user: {Username}", user.Username);
        return Ok(new { token });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An error occurred while logging in.");
        return StatusCode(500, new { message = "An internal server error occurred." });
    }
}

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim("id", user.UserId.ToString()), 
                    new Claim(ClaimTypes.Role, user.Role.ToString()) 
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
