using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Domain;
using TaskManager.Service.Interface;

namespace TaskManagerAPI.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : BaseApiController
    {
        public UploadController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : base(repository, logger, mapper)
        {
        }

        [HttpPost("uploadPhoto/{userId}")]
        public async Task<ApiResponse<string>> UploadUserPhoto(string userId,IFormFile photo)
        {
            var result = await _repository.UploadManager.UploadPhoto(userId, photo);
            return result;
        }

        [HttpPost("uploadFiles/{taskId}")]
        public async Task<ApiResponse> UploadTaskFiles(int taskId, List<IFormFile> files)
        {
            var result = await _repository.UploadManager.UploadFiles(taskId, files);
            return result;
        }

        [HttpGet("getTaskFiles/{taskId}")]
        public async Task<ApiResponse<List<TaskFiles>>> GetTaskById(int taskId)
        {
            var task = await _repository.UploadManager.GetFilesByTaskId(taskId, trackChanges: false);
            return task;
            
        }

        [HttpPost("deleteFile/{Id}")]
        public async Task<ApiResponse> UploadTaskFiles(int Id)
        {
            var result = await _repository.UploadManager.DeleteFiles(Id, trackChanges: false);
            return result;
        }
    }
}
