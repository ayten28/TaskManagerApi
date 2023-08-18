using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Domain;
using TaskManager.Core.DTOs;
using TaskManager.Service.Filters;
using TaskManager.Service.Interface;

namespace TaskManagerAPI.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : BaseApiController
    {
        public CommentsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : base(repository, logger, mapper)
        {
        }

        [HttpPost]
        public async Task<ApiResponse<Comment>> CreateTask([FromBody] AddCommentDto comment)
        {
            Comment commentdata = new Comment();
            var result = new ApiResponse<Comment>(commentdata);
            try
            {
                commentdata = _mapper.Map<Comment>(comment);
                await _repository.Comments.AddComment(commentdata);
                await _repository.SaveAsync();

                result.Data = commentdata;
                result.Message = "Success";
                result.IsSuccess = true;
                result.Code = TaskManager.Core.Response.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Comment Creation failed. Exception: " + ex.Message;
                result.Code = TaskManager.Core.Response.ResponseCode.Failed;
            }

            return result;            
        }

        [HttpGet("{taskId}", Name = "getComments")]
        public async Task<ApiResponse<List<CommentsDto>>> GetComments(int taskId)
        {
            var comments = await _repository.Comments.GetComments(taskId, trackChanges: false);
            return comments;
        }

        [HttpPut("deleteComment/{id}")]
        [ServiceFilter(typeof(ValidateCommentExists))]
        public async Task<ApiResponse> DeleteComment(int id)
        {
            var result = await _repository.Comments.DeleteComment(id);
            if(result.IsSuccess) await _repository.SaveAsync();
            return result;
        }

        [HttpPut("updateComment/{id}&{userId}&{text}")]
        [ServiceFilter(typeof(ValidateCommentExists))]
        public async Task<ApiResponse<Comment>> UpdateComment(int id, string userId, string text)
        {
            var result = await _repository.Comments.UpdateComment(id, userId, text);
            if (result.IsSuccess) {
                try
                {
                    var commentData = HttpContext.Items["comment"] as Comment;
                    var aaa = _mapper.Map(result.Data, commentData);
                    
                    await _repository.SaveAsync();
                }
                catch (Exception ex) 
                {
                    result.Code = TaskManager.Core.Response.ResponseCode.Failed;
                    result.IsSuccess = false;
                    result.Message = ex.Message;
                }
            }
         
            return result;
        }
    }
}
