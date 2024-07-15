using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // Bu satırı ekleyin

namespace TaskManagerProject.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly DatabaseContext _context;

        public TaskController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Tasks.Include(t => t.AssignedUser).Include(t => t.Project).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["Users"] = new SelectList(_context.Users, "UserId", "Username");
            ViewData["Projects"] = new SelectList(_context.Projects, "ProjectId", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Create([Bind("TaskId,Title,Description,DueDate,AssignedUserId,ProjectId,Status")] TaskManagerProject.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Users"] = new SelectList(_context.Users, "UserId", "Username", task.AssignedUserId);
            ViewData["Projects"] = new SelectList(_context.Projects, "ProjectId", "Title", task.ProjectId);
            return View(task);
        }

        public async System.Threading.Tasks.Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["Users"] = new SelectList(_context.Users, "UserId", "Username", task.AssignedUserId);
            ViewData["Projects"] = new SelectList(_context.Projects, "ProjectId", "Title", task.ProjectId);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Edit(int id, [Bind("TaskId,Title,Description,DueDate,AssignedUserId,ProjectId,Status")] TaskManagerProject.Models.Task task)
        {
            if (id != task.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Users"] = new SelectList(_context.Users, "UserId", "Username", task.AssignedUserId);
            ViewData["Projects"] = new SelectList(_context.Projects, "ProjectId", "Title", task.ProjectId);
            return View(task);
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}
