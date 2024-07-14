using Microsoft.AspNetCore.Mvc;
using TaskManagerProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace TaskManagerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public UserController(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username, // Kullanıcı adı atanıyor
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password), // Şifre hashleniyor
                Email = userDto.Email, // Kullanıcı emaili
                FirstName = userDto.FirstName, // Kullanıcı adı
                LastName = userDto.LastName, // Kullanıcı soyadı
                CreatedDate = DateTime.UtcNow, // Oluşturulma tarihi
                UpdatedDate = DateTime.UtcNow, // Güncellenme tarihi
                Role = UserRole.Employee // Varsayılan rol: Employee
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }


        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()), new Claim(ClaimTypes.Role, user.Role.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPut("updateProfile")]
        public async Task<IActionResult> UpdateProfile(UserDto userDto)
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
