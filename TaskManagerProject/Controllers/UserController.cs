using Microsoft.AspNetCore.Mvc;
using TaskManagerProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Data;

namespace TaskManagerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;

        // _configuration'ı başlatmak için constructor'ı güncelledik
        public UserController(DatabaseContext context, IConfiguration configuration, ILogger<UserController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
                {
                    return BadRequest(new { message = "Username is already taken" });
                }

                if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                {
                    return BadRequest(new { message = "Email is already registered" });
                }

                var user = new User
                {
                    Username = userDto.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                    Email = userDto.Email,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    Role = Role.User // Varsayılan olarak kullanıcı rolü
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering a user.");
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
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
            var secretKey = _configuration["AppSettings:Secret"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("Secret key is not defined in AppSettings.");
            }
            var key = Encoding.ASCII.GetBytes(secretKey);
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

        [HttpPut("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDto userDto)
        {
            var user = await _context.Users.FindAsync(userDto.UserId);
            if (user == null)
            {
                return NotFound(); // Kullanıcı bulunamadı hatası
            }

            user.Username = userDto.Username; // Kullanıcı adı güncelleniyor
            user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password); // Şifre güncelleniyor ve hashleniyor
            user.Email = userDto.Email; // Email güncelleniyor
            user.FirstName = userDto.FirstName; // Ad güncelleniyor
            user.LastName = userDto.LastName; // Soyad güncelleniyor
            user.UpdatedDate = DateTime.UtcNow; // Güncellenme tarihi ayarlanıyor

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}
