using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TaskManager.Core.Domain;
using TaskManager.Data.Data;
using TaskManager.Data.GenericRepository.Service;
using TaskManager.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Service.Service
{
    public class UploadManager : RepositoryBase<TaskFiles>, IUploadManager
    {
        private IUserAuthenticationRepository _userAuthenticationRepository;
        private IConfiguration _configuration;
        private RepositoryContext _repositoryContext;


        public UploadManager(RepositoryContext repositoryContext,
            IUserAuthenticationRepository userAuthenticationRepository, IConfiguration configuration)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
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

        public async System.Threading.Tasks.Task AddFile(TaskFiles file) => await CreateAsync(file);

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

        public async Task<ApiResponse> UploadFiles(int taskId, List<IFormFile> files)
        {
            var result = new ApiResponse();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = _configuration.GetSection("Path").GetSection("FilePath").Value;


                    filePath = Path.Combine(filePath, "uploadFiles", taskId.ToString());
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    try {
                        filePath = Path.Combine(filePath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var fileEntity = new TaskFiles
                        {
                            TaskId = taskId,
                            FileName = fileName,
                            FilePath = filePath
                        };

                        try { 
                        await CreateAsync(fileEntity);
                        await _repositoryContext.SaveChangesAsync();
                        
                        }
                        catch (Exception ex) {
                        result.IsSuccess = false;
                        result.Message = ex.Message;
                        result.Code = Core.Response.ResponseCode.Failed;
                    }
                        
                        //repositoryContext.Files.Add(fileEntity);
                    }
                    catch (Exception ex) {
                        result.IsSuccess = false;
                        result.Message = ex.Message;
                        result.Code = Core.Response.ResponseCode.Failed;
                    }                    
                }
            }

            return result;
        }

        public async 
            Task<ApiResponse<List<TaskFiles>>> GetFilesByTaskId(int taskId, bool trackChanges)
        {
             var taskFiles = await FindByConditionAsync(c => c.TaskId.Equals(taskId), trackChanges).Result.OrderByDescending(c => c.Id)
       .ToListAsync();

              var result = new ApiResponse<List<TaskFiles>>(taskFiles);
            
            if (taskFiles == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }

            return result;
        }

        public async Task<ApiResponse> DeleteFiles(int Id, bool trackChanges)
        {
            var result = new ApiResponse();
            var file = await FindByConditionAsync(c => c.Id.Equals(Id), trackChanges).Result.SingleOrDefaultAsync();
            if (file != null) 
            {
                try {
                    if (File.Exists(file.FilePath))
                    {
                        File.Delete(file.FilePath);
                    }
                    await RemoveAsync(file);
                    await _repositoryContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = ex.Message;
                    result.Code = Core.Response.ResponseCode.Failed;
                }
            }
            else {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            return result;
        }
    }
    }


