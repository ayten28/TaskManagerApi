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
    }
}
