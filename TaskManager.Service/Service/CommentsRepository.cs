using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Domain;
using TaskManager.Core.DTOs;
using TaskManager.Data.Data;
using TaskManager.Data.GenericRepository.Service;
using TaskManager.Service.Interface;

namespace TaskManager.Service.Service
{
    public class CommentsRepository : RepositoryBase<Comment>, ICommentsRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public CommentsRepository(RepositoryContext repositoryContext, UserManager<User> userManager,
            IMapper mapper) : base(repositoryContext)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        public async System.Threading.Tasks.Task AddComment(Comment comment) => await CreateAsync(comment);

        public async Task<ApiResponse> DeleteComment(int commentId)
        {
            var response = new ApiResponse();
            try
            {
                var comment = await GetCommentById(commentId, true);
                await RemoveAsync(comment);
            }
            catch (Exception ex) 
            {
                response.Code = Core.Response.ResponseCode.Failed;
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Comment> GetCommentById(int id, bool trackChanges)
        {
            var comment = await FindByConditionAsync(c => c.Id.Equals(id), trackChanges).Result.SingleOrDefaultAsync();
            return comment;
        }

        public async Task<ApiResponse<List<CommentsDto>>> GetComments(int taskId, bool trackChanges)
        {
            var comments = await FindByConditionAsync(c => c.TaskId.Equals(taskId.ToString()), trackChanges)
                .Result.OrderByDescending(c => c.CreatedDate).ToListAsync();
            var commentsDto = MapComments(comments);
            var result = new ApiResponse<List<CommentsDto>>(commentsDto);
            if (comments == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            return result;
        }

        public async Task<ApiResponse<Comment>> UpdateComment(int id, string userId, string text)
        {
            
            var comment = await FindByConditionAsync(c => c.Id.Equals(id) && c.CreateUser.Equals(userId), true).Result.SingleOrDefaultAsync();
            var response = new ApiResponse<Comment>(comment);
            if (comment != null)
            {
                comment.Text = text;
                try
                {
                    var mapped = _mapper.Map<Comment>(comment);
                    response.Data = comment;
                   // _mapper.Map(comment, );
                    //await UpdateAsync(mapped);
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
                response.IsSuccess = false;
                response.Message = "Data could not be found";
                response.Code = Core.Response.ResponseCode.DataNotFound;
            }
            return response;
        }

        protected List<CommentsDto> MapComments(List<Comment> comments)
        {
            var commentsDto = comments.Join(_userManager.Users,
                   comment => comment.CreateUser,
                   user => user.Id,
                   (comment, user) => new CommentsDto
                   {
                       Id = comment.Id,
                       CreatedDate = comment.CreatedDate,
                       Text = comment.Text,
                       TaskId = comment.TaskId,
                       CreateUser = comment.CreateUser,
                       CreateUserLogin = user.UserName,
                       CreateUserFullName = user.FirstName + ' ' + user.LastName
                   }).ToList();
            return commentsDto;
        }
    }
}
