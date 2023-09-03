using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Domain;
using TaskManager.Service.Interface;

namespace TaskManager.Service.Service
{
    public class UploadManager : IUploadManager
    {
        private IUserAuthenticationRepository _userAuthenticationRepository;
        private IConfiguration _configuration;
        public UploadManager(IUserAuthenticationRepository userAuthenticationRepository, IConfiguration configuration)
        {
            _userAuthenticationRepository = userAuthenticationRepository; 
            _configuration = configuration;
        }

        public async Task<ApiResponse<string>> UploadPhoto(string userId, IFormFile photo)
        {
            var url = "";
            var result = new ApiResponse<string>(url);
            if (photo == null || photo.Length <= 0)
            {
                result.Code = Core.Response.ResponseCode.Failed;
                result.IsSuccess = false;
                result.Message = "No photo uploaded.";
            }
            else
            {
                result = await StorePhotoAsync(photo, userId);
            }
            
            return result;
        }

        private async Task<ApiResponse<string>> StorePhotoAsync(IFormFile photo, string id)
        {
            var photoUrl = "";
            var result = new ApiResponse<string>(photoUrl);
            
            var fileName = Path.GetFileName(photo.FileName);
            var filePath = _configuration.GetSection("Path").GetSection("FilePath").Value;


            filePath = Path.Combine(filePath,"uploads");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            
            try
            {
                filePath = Path.Combine(filePath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }
                var updateResult = await _userAuthenticationRepository.UpdatePhotoUrl(id, filePath);

                photoUrl = $"https://your-api-endpoint.com/{filePath}";
            }
            catch (Exception ex) 
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                result.Code = Core.Response.ResponseCode.Failed;
            }
            
            return result;
        }
    }
}

