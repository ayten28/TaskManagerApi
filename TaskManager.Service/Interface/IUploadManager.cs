using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Domain;

namespace TaskManager.Service.Interface
{
    public interface IUploadManager
    {
        Task<ApiResponse<string>> UploadPhoto(string userId, IFormFile photo);
    }
}
