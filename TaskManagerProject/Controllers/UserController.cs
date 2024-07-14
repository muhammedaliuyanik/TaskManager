using Microsoft.AspNetCore.Mvc;
using TaskManagerProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TaskManagerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UserController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            // Registration logic
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            // Login logic
            return Ok();
        }

        [HttpPut("updateProfile")]
        public async Task<IActionResult> UpdateProfile(User user)
        {
            // Profile update logic
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            // User update logic
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
