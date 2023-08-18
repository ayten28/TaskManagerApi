using TaskManager.Data.Data;
using TaskManager.Data.GenericRepository.Service;
using TaskManager.Service.Interface;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Domain;
using TaskManager.Core.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace TaskManager.Service.Service
{
    public class TaskRepository : RepositoryBase<Core.Domain.Task>, ITaskRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public TaskRepository(RepositoryContext repositoryContext, IMapper mapper,
             UserManager<User> userManager) : base(repositoryContext)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        protected List<TaskDto> MapTask(List<Core.Domain.Task> tasks)
        {
            var tasksDto = tasks.Join(_userManager.Users,
                   task => task.CreateUser,
                   user => user.Id,
                   (task, user) => new
                   {
                       Task = task,
                       CreateUser = task.CreateUser,
                       CreateUserLogin = user.UserName,
                       CreateUserFullName = user.FirstName + ' ' + user.LastName
                   }).Join(_userManager.Users,
                   taskUserJoin => taskUserJoin.Task.AssignUser,
                   user => user.Id,
                   (taskUserJoin, user) => new TaskDto
                   {
                       Id = taskUserJoin.Task.Id,
                       TaskName = taskUserJoin.Task.TaskName,
                       TaskDescription = taskUserJoin.Task.TaskDescription,
                       CreatedDate = taskUserJoin.Task.CreatedDate,
                       IsDone = taskUserJoin.Task.IsDone,
                       ResolveDate = taskUserJoin.Task.ResolveDate,
                       CreateUser = taskUserJoin.CreateUser,
                       CreateUserLogin = taskUserJoin.CreateUserLogin,
                       CreateUserFullName = taskUserJoin.CreateUserFullName,
                       AssignUser = taskUserJoin.Task.AssignUser,
                       AssignUserLogin = user.UserName,
                       AssignUserFullName = user.FirstName + ' ' + user.LastName
                   }).ToList();
            return tasksDto;
        }       
        public async System.Threading.Tasks.Task UpdateTask(Core.Domain.Task task) => await UpdateAsync(task);

        public async Task<ApiResponse<List<TaskDto>>> GetAllTasks(bool trackChanges)
        {
            var tasks = await FindAllAsync(trackChanges).Result.OrderBy(c => c.CreatedDate).ToListAsync();
            var tasksDto = MapTask(tasks);
            var result = new ApiResponse<List<TaskDto>>(tasksDto);
            if (tasks == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            return result;
        }

        public async Task<ApiResponse<TaskDto>> GetTask(int taskId, bool trackChanges)
        {
            var tasks = await FindByConditionAsync(c => c.Id.Equals(taskId), trackChanges).Result.ToListAsync();
            var tasksDto = MapTask(tasks);
            var taskDto = tasksDto.FirstOrDefault();
            var result = new ApiResponse<TaskDto>(taskDto);
            if (result == null) 
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.IsSuccess = false;
                result.Message = "Data could not be found";
            }
            return result;
        }


        public async Task<Core.Domain.Task> GetTaskById(int taskId, bool trackChanges)
        {
            var task = await FindByConditionAsync(c => c.Id.Equals(taskId), trackChanges).Result.SingleOrDefaultAsync();            
            return task;
        }


        public async Task<ApiResponse<List<TaskDto>>> GetMyDoneCreatedTask(string createUser, bool trackChanges)
        {
            var tasks = await FindByConditionAsync(c => c.CreateUser.Equals(createUser) && c.IsDone == true, trackChanges)
                .Result.OrderByDescending(c => c.CreatedDate).ToListAsync();
            var tasksDto = MapTask(tasks);
            var result = new ApiResponse<List<TaskDto>>(tasksDto);
            if (tasks == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            return result;
        }

        public async Task<ApiResponse<List<TaskDto>>> GetMyDoneAssignedTask(string assignedUser, bool trackChanges)
        {
            var tasks = await FindByConditionAsync(c => c.AssignUser.Equals(assignedUser) && c.IsDone == true, trackChanges).Result.OrderByDescending(c => c.CreatedDate).ToListAsync();
            var tasksDto = MapTask(tasks);
            var result = new ApiResponse<List<TaskDto>>(tasksDto);
            if (tasks == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            return result;
        }

        public async Task<ApiResponse<List<TaskDto>>> GetMyAssignnedTask(string assignedUser, bool trackChanges)
        {
            var tasks = await FindByConditionAsync(c => c.AssignUser.Equals(assignedUser), trackChanges).Result.OrderByDescending(c => c.CreatedDate).ToListAsync();
            var tasksDto = MapTask(tasks);
            var result = new ApiResponse<List<TaskDto>>(tasksDto);
            if (tasks == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            return result;
        }

        public async Task<ApiResponse<List<TaskDto>>> GetMyCreatedTask(string createUser, bool trackChanges)
        {
            var tasks = await FindByConditionAsync(c => c.CreateUser.Equals(createUser), trackChanges).Result.OrderByDescending(c => c.CreatedDate).ToListAsync();
            var tasksDto = MapTask(tasks);
            var result = new ApiResponse<List<TaskDto>>(tasksDto);
            if (tasks == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            return result;
        }
        public async System.Threading.Tasks.Task CreateTask(Core.Domain.Task task) => await CreateAsync(task);

        public async Task<ApiResponse> ResolveTask(int id)
        {
            var response = new ApiResponse();
            var task = await GetTaskById(id, true);
            if (task != null)
            {
                try
                {
                    task.IsDone = true;
                    await RepositoryContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    response.Code = Core.Response.ResponseCode.Failed;
                    response.IsSuccess = false;
                    response.Message = ex.Message;
                }
            }
            else 
            {
                response.Code = Core.Response.ResponseCode.DataNotFound;
                response.IsSuccess = false;
                response.Message = $"Task with ID: {id} couldnot be found";
            }
            return response;
        }

    }
}
