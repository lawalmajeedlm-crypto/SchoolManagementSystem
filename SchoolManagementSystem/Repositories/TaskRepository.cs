using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.TaskItems
                .Include(t => t.User)  // Eager loading for User
                .Include(t => t.CreatedBy)  // Eager loading for auditing
                .Include(t => t.UpdatedBy)
                .ToListAsync();
        }

        public async Task<TaskItem> GetByIdAsync(int id)
        {
            return await _context.TaskItems
                .Include(t => t.User)  // Eager loading
                .Include(t => t.CreatedBy)
                .Include(t => t.UpdatedBy)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetByUserIdAsync(string userId)
        {
            return await _context.TaskItems
                .Where(t => t.UserId == userId)
                .Include(t => t.User)  // Eager loading
                .Include(t => t.CreatedBy)
                .Include(t => t.UpdatedBy)
                .ToListAsync();
        }

        public async Task AddAsync(TaskItem task)
        {
            await _context.TaskItems.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task != null)
            {
                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.TaskItems.AnyAsync(t => t.Id == id);
        }
    }
}