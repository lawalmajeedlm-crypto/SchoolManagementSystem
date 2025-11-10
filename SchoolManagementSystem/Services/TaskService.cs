using SchoolManagementSystem.Models;
using SchoolManagementSystem.Repositories;
using SchoolManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskItem>> GetTasksForUserAsync(string userId, bool isAdmin)
        {
            return isAdmin ? await _taskRepository.GetAllAsync() : await _taskRepository.GetByUserIdAsync(userId);
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id, string userId, bool isAdmin)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) throw new KeyNotFoundException("Task not found.");
            if (!isAdmin && task.UserId != userId) throw new UnauthorizedAccessException("Access denied.");
            return task;
        }

        public async Task CreateTaskAsync(TaskItem task, string userId)
        {
            if (string.IsNullOrEmpty(task.Title)) throw new ArgumentException("Title is required.");
            task.UserId = userId;
            task.CreatedById = userId;
            await _taskRepository.AddAsync(task);
        }

        public async Task UpdateTaskAsync(int id, TaskItem task, string userId, bool isAdmin)
        {
            var existingTask = await GetTaskByIdAsync(id, userId, isAdmin);
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.IsCompleted = task.IsCompleted;
            existingTask.UpdatedAt = DateTime.UtcNow;
            existingTask.UpdatedById = userId;
            await _taskRepository.UpdateAsync(existingTask);
        }

        public async Task DeleteTaskAsync(int id, string userId, bool isAdmin)
        {
            var task = await GetTaskByIdAsync(id, userId, isAdmin);
            await _taskRepository.DeleteAsync(id);
        }
    }
}