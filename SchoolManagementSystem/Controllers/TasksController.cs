using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    [Authorize] // ✅ Only logged-in users can access tasks
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.TaskItems
                .Include(t => t.User) // ✅ load related user
                .ToListAsync();

            return View(tasks);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var taskItem = await _context.TaskItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (taskItem == null) return NotFound();

            return View(taskItem);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                // ✅ Automatically assign the logged-in user
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                taskItem.UserId = userId;

                _context.Add(taskItem);
                await _context.SaveChangesAsync(); // auditing handled in DbContext override
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }


        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null) return NotFound();

            return View(taskItem);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItem taskItem)
        {
            if (id != taskItem.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // ✅ Preserve UserId (don’t overwrite ownership)
                    var existingTask = await _context.TaskItems.AsNoTracking()
                        .FirstOrDefaultAsync(t => t.Id == id);

                    if (existingTask == null) return NotFound();

                    taskItem.UserId = existingTask.UserId;

                    _context.Update(taskItem);
                    await _context.SaveChangesAsync(); // UpdatedById handled in DbContext override
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var taskItem = await _context.TaskItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (taskItem == null) return NotFound();

            return View(taskItem);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
    }
}
