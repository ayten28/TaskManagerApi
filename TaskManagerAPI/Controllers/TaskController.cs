using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.DTOs;
using TaskManager.Service.Interface;
using TaskManager.Core.Domain;
using TaskManager.Service.Filters;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagerAPI.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : BaseApiController
    {
        public TaskController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : base(repository, logger, mapper)
        {
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ApiResponse<TaskManager.Core.Domain.Task>> CreateTask([FromBody] TaskCreationDto task)
        {
            var taskdata = _mapper.Map<TaskManager.Core.Domain.Task>(task);
            await _repository.Task.CreateTask(taskdata);
            await _repository.SaveAsync();
            //var taskReturn = _mapper.Map<TaskDto>(taskdata);
            var result = new ApiResponse<TaskManager.Core.Domain.Task>(taskdata);
            result.Message = "Success";
            return result;

            //return CreatedAtRoute("TaskById",
            //    new
            //    {
            //        taskId = taskReturn.Id
            //    },
            //    taskReturn);
        }

        [HttpGet]
        [Authorize]
        //[ResponseCache(CacheProfileName = "30SecondsCaching")]
        public async Task<ApiResponse<List<TaskDto>>> GetAllTasks()
        {
            return await _repository.Task.GetAllTasks(trackChanges: false);          
        }

        [HttpGet("{taskId}", Name = "TaskById")]
        [ServiceFilter(typeof(ValidateTaskExists))]
        public async Task<ApiResponse<TaskDto>> GetTaskById(int taskId)
        {
            var task = await _repository.Task.GetTask(taskId, trackChanges: false);
            return task;
            //if (task is null)
            //{
            //    _logger.LogInfo($"Task with id: {taskId} doesn't exist in the database.");
            //    return NotFound();
            //}
            //else
            //{
            //    var taskDto = _mapper.Map<TaskDto>(task);
            //    return Ok(taskDto);
            //}
        }

        [HttpGet("MyTasks/{createUser}", Name = "MyCreatedTasks")]
        public async Task<ApiResponse<List<TaskDto>>> GetMyCreatedTasks(string createUser)
        {
            var task = await _repository.Task.GetMyCreatedTask(createUser, trackChanges: false);
            return task;
        }


        [HttpGet("MyDoneTasks/{createUser}", Name = "MyCreatedDoneTasks")]
        public async Task<ApiResponse<List<TaskDto>>> GetMyCreatedDoneTasks(string createUser)
        {
            var tasks = await _repository.Task.GetMyDoneCreatedTask(createUser, trackChanges: false);
            return tasks;
        }

        [HttpGet("AssignedTasks/{assignedUser}", Name = "AssignedTasks")]
        public async Task<ApiResponse<List<TaskDto>>> GetAssignedTasks(string assignedUser)
        {
            var task = await _repository.Task.GetMyAssignnedTask(assignedUser, trackChanges: false);
            return task;
        }

        [HttpGet("AssignedDoneTasks/{assignedUser}", Name = "AssignedDoneTasks")]
        public async Task<ApiResponse<List<TaskDto>>> GetAssignedDoneTasks(string assignedUser)
        {
            var task = await _repository.Task.GetMyDoneAssignedTask(assignedUser, trackChanges: false);
            return task;
        }

        [HttpPut("{taskId}")]
       // [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTaskExists))]
        public async Task<ApiResponse> UpdateTask(int taskId, [FromBody] TaskUpdateDto task)
        {
            var response = new ApiResponse();
            try
            {
                var taskData = HttpContext.Items["task"] as TaskManager.Core.Domain.Task;
                _mapper.Map(task, taskData);
                //  await _repository.Task.UpdateTask(taskData);                
                await _repository.SaveAsync();               
                response.IsSuccess = true;
                response.Message = "Successfuly updated";
                response.Code = TaskManager.Core.Response.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Update failed. Exception: " + ex.Message;
                response.Code = TaskManager.Core.Response.ResponseCode.Failed;
            }          
            
            return response;
        }

    }
}
