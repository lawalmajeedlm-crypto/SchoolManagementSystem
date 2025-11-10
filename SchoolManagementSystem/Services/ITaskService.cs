using SchoolManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetTasksForUserAsync(string userId, bool isAdmin);
        Task<TaskItem> GetTaskByIdAsync(int id, string userId, bool isAdmin);
        Task CreateTaskAsync(TaskItem task, string userId);
        Task UpdateTaskAsync(int id, TaskItem task, string userId, bool isAdmin);
        Task DeleteTaskAsync(int id, string userid, bool isAdmin);
    }
}
