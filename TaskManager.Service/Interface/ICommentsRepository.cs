using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Domain;
using TaskManager.Core.DTOs;

namespace TaskManager.Service.Interface
{
    public interface ICommentsRepository
    {
        System.Threading.Tasks.Task AddComment(Comment comment);
        Task<ApiResponse<List<CommentsDto>>> GetComments(int taskId, bool trackChanges);
        Task<Comment> GetCommentById(int id, bool trackChanges);
        Task<ApiResponse> DeleteComment(int commentId);
        Task<ApiResponse<Comment>> UpdateComment(int id, string userIds, string text);

    }
}
