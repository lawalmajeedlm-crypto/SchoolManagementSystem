using SchoolManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem> GetByIdAsync(int id);
        Task<IEnumerable<TaskItem>> GetByUserIdAsync(string userId);
        Task AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}