using Microsoft.AspNetCore.Mvc;
using TaskManagerProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace TaskManagerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProjectController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            project.CreatedDate = DateTime.UtcNow;
            project.UpdatedDate = DateTime.UtcNow;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Project created successfully" });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project project)
        {
            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject == null)
            {
                return NotFound(new { message = "Project not found" });
            }

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            existingProject.UpdatedDate = DateTime.UtcNow;

            _context.Entry(existingProject).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Project updated successfully" });
        }
    }
}
