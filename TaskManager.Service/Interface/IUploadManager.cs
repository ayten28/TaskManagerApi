using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Domain;
using TaskManager.Core.DTOs;

namespace TaskManager.Service.Interface
{
    public interface IUploadManager
    {
        Task<ApiResponse<string>> UploadPhoto(string userId, IFormFile photo);
        Task<ApiResponse> UploadFiles(int taskId, List<IFormFile> files);
        Task<ApiResponse<List<TaskFiles>>> GetFilesByTaskId(int taskId, bool trackChanges);
        Task<ApiResponse> DeleteFiles(int Id, bool trackChanges);
    }
}
