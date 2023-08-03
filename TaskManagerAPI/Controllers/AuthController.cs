using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Domain;
using TaskManager.Core.DTOs;
using TaskManager.Core.DTOs.Responses;
using TaskManager.Service.Filters;
using TaskManager.Service.Interface;

namespace TaskManagerAPI.Controllers
{
    [Route("api/userauthentication")]
    [ApiController]
    [HandleUnauthorized]
    public class AuthController : BaseApiController
    {
        public AuthController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : base(repository, logger, mapper)
        {
        }

        [HttpPost]
       // [Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ApiResponse<RegisterResonseDto>> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            var userResult = await _repository.UserAuthentication.RegisterUserAsync(userRegistration);
            return userResult;
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ApiResponse<LoginResponseDto>> Authenticate([FromBody] UserLoginDto user)
        {
            LoginResponseDto loginResponseDto = new LoginResponseDto();
            var result = new ApiResponse<LoginResponseDto>(loginResponseDto);

            if (!await _repository.UserAuthentication.ValidateUserAsync(user))
            { 
                result.Code = TaskManager.Core.Response.ResponseCode.UnAuthorized;
                result.Message = "UnAuthorized";
                result.IsSuccess = false;
            }
            else
            {
                loginResponseDto.Token = await _repository.UserAuthentication.CreateTokenAsync();
            }
            return result;
        }

        [HttpGet("getUsersList")]
        [Authorize]
        public async Task<ApiResponse<List<User>>> GetAllUsers()
        {
                var users = await _repository.UserAuthentication.GetUserListAsync();
                //var userDto = _mapper.Map<List<UserListDto>>(users);
                return users;
            
        }

        [HttpGet("{userId}", Name = "GetUserInfo")]
        [Authorize]
        public async Task<ApiResponse<User>> GetUserById(string userId) {           
            var user = await _repository.UserAuthentication.GetUserById(userId);
            //var userDto = _mapper.Map<List<UserListDto>>(users);
            return user;
        }

        [HttpPost("update/{userId}", Name = "Update")]
        [Authorize]
        public async Task<ApiResponse<User>> UpdateUser(string userId, [FromBody] UserUpdateDto user) 
        {
            var result = await _repository.UserAuthentication.UpdateUserInfo(userId, user);
            return result;
        }

        [HttpPost("block/{userId}", Name = "Block")]
        [Authorize]
        public async Task<ApiResponse> BlockUser(string userId)
        {
            var result = await _repository.UserAuthentication.BlockUser(userId);
            return result;
        }

        [HttpPost("changePassword/{userId}", Name = "ChangePassword")]
        [Authorize]
        public async Task<ApiResponse> UpdateUserPassword(string userId, [FromBody] UpdateUserPasswordDto model)
        {
            var result = await _repository.UserAuthentication.ChanngePassword(userId, model);
            return result;
        }

        [HttpGet("getUsersListExceptCurrent/{userId}")]
        //[Authorize]
        public async Task<ApiResponse<List<User>>> GetUsersListExceptCurrent(string userId)
        {
            var users = await _repository.UserAuthentication.GetUserListExceptCurrentAsync(userId);
            return users;

        }
    }
}
