using Microsoft.AspNetCore.Identity;
using TaskManager.Core.Domain;
using TaskManager.Core.DTOs;
using TaskManager.Core.DTOs.Responses;

namespace TaskManager.Service.Interface
{
    public interface IUserAuthenticationRepository
    {
        Task<ApiResponse<RegisterResonseDto>> RegisterUserAsync(UserRegistrationDto userForRegistration);
        Task<bool> ValidateUserAsync(UserLoginDto loginDto);
        Task<string> CreateTokenAsync();         
        Task<ApiResponse<List<User>>> GetUserListAsync();
        Task<ApiResponse<List<User>>> GetUserListExceptCurrentAsync(string id);
        Task<ApiResponse<User>> GetUserById(string id);
        Task<ApiResponse<User>> UpdateUserInfo(string id, UserUpdateDto user);
        Task<ApiResponse> BlockUser(string id);
        Task<ApiResponse> ChanngePassword(string id, UpdateUserPasswordDto pswd);
        Task<ApiResponse> UpdatePhotoUrl(string id, string url);

        //Task<List<User>> GetUserListExcept(string currenUser);
    }
}
