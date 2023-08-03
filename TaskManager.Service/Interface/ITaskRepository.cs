using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Domain;
using TaskManager.Core.DTOs;

namespace TaskManager.Service.Interface
{
    public interface ITaskRepository
    {
        Task<ApiResponse<List<TaskDto>>> GetAllTasks(bool trackChanges);
        Task<ApiResponse<TaskDto>> GetTask(int taskId, bool trackChanges);
        Task<Core.Domain.Task> GetTaskById(int taskId, bool trackChanges);
        System.Threading.Tasks.Task CreateTask(Core.Domain.Task task);
        Task<ApiResponse<List<TaskDto>>> GetMyCreatedTask(string createUser, bool trackChanges);
        Task<ApiResponse<List<TaskDto>>> GetMyDoneCreatedTask(string createUser, bool trackChanges);
        Task<ApiResponse<List<TaskDto>>> GetMyAssignnedTask(string assignedUser, bool trackChanges);
        Task<ApiResponse<List<TaskDto>>> GetMyDoneAssignedTask(string assignedUser, bool trackChanges);
        System.Threading.Tasks.Task UpdateTask(Core.Domain.Task task);

        //Task<IActionResult> CreateTask2(Core.Domain.Task task);
    }
}
