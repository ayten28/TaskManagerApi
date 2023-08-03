using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Domain;
using TaskManager.Core.DTOs;
using TaskManager.Core.DTOs.Responses;
using TaskManager.Service.Interface;

namespace TaskManager.Service.Service
{
    internal sealed class UserAuthenticationRepository : IUserAuthenticationRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private User? _user;
        private ILoggerManager _logger;
        public UserAuthenticationRepository(
        UserManager<User> userManager, Microsoft.Extensions.Configuration.IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ApiResponse<RegisterResonseDto>> RegisterUserAsync(UserRegistrationDto userRegistration)
        {
            var user = _mapper.Map<User>(userRegistration);
            var data = await _userManager.CreateAsync(user, userRegistration.Password);
            var id = user.Id;
            RegisterResonseDto response = new RegisterResonseDto();            
           
            var result = new ApiResponse<RegisterResonseDto>(response);
            if (!data.Succeeded) {                
                result.Code = Core.Response.ResponseCode.Failed;
                result.IsSuccess = false;
                foreach (var error in data.Errors)
                    result.Message += ' ' + error.Description;
                //_logger.LogError($"Something went wrong in the {nameof(RegisterUserAsync)} action {result.Message}");
            }
            else response.Id = id;
                return result;
        }

        public async Task<bool> ValidateUserAsync(UserLoginDto loginDto)
        {
            _user = await _userManager.FindByNameAsync(loginDto.UserName);
            var result = _user != null && await _userManager.CheckPasswordAsync(_user, loginDto.Password);
            return result;
        }

        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("jwtConfig");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
        {
            new Claim("UserName", _user.UserName),
            new Claim("UserId", _user.Id)
        };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("Roles", role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims.ToArray(),
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiresIn"])),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        public async Task<ApiResponse<List<User>>> GetUserListAsync()
        {            
            var users = await _userManager.Users.Where(u => u.Blocked == false).ToListAsync();
       
            var result = new ApiResponse<List<User>>(users);
            if (users == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            return result;
        }

        public async Task<ApiResponse<User>> GetUserById(string userId) 
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId && u.Blocked == false);
                
            var result = new ApiResponse<User>(user);
            if (user == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            return result;
           // _logger.LogError($"Something went wrong in the {nameof(GetUserById)} action {ex}");
        }

        public async Task<ApiResponse<User>> UpdateUserInfo(string id, UserUpdateDto userUpdate)
        {
            var userExists = await _userManager.FindByIdAsync(id);
            var result = new ApiResponse<User>(userExists);
            if (userExists == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            else {
                userExists.LastName = userUpdate.LastName;
                userExists.FirstName = userUpdate.FirstName;
                userExists.PhoneNumber = userUpdate.PhoneNumber;
                userExists.Gender = userUpdate.Gender;
                userExists.Position = userUpdate.Position;
                var data = await _userManager.UpdateAsync(userExists);
                if (!data.Succeeded)
                {
                    result.Code = Core.Response.ResponseCode.Failed;
                    result.IsSuccess = false;
                    foreach (var error in data.Errors)
                        result.Message += ' ' + error.Description;
                }
                else {
                    result.Code = Core.Response.ResponseCode.Success;
                   
                }
            }
            return result;
        }

        public async Task<ApiResponse> BlockUser(string id)
        {
            var userExists = await _userManager.FindByIdAsync(id);
            var result = new ApiResponse();
            if (userExists == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            else
            {
                userExists.Blocked = true;
                var data = await _userManager.UpdateAsync(userExists);
                if (!data.Succeeded)
                {
                    result.Code = Core.Response.ResponseCode.Failed;
                    result.IsSuccess = false;
                    foreach (var error in data.Errors)
                        result.Message += ' ' + error.Description;
                }
                else
                {
                    result.Code = Core.Response.ResponseCode.Success;

                }                
            }
            return result;
        }

        public async Task<ApiResponse> ChanngePassword(string id, UpdateUserPasswordDto pswd)
        {
            var userExists = await _userManager.FindByIdAsync(id);
            var result = new ApiResponse();
            if (userExists == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            else
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(userExists, pswd.CurrentPassword, pswd.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    result.Code = Core.Response.ResponseCode.Failed;
                    result.IsSuccess = false;
                    foreach (var error in changePasswordResult.Errors)
                        result.Message += ' ' + error.Description;
                }
                else
                {
                    result.Code = Core.Response.ResponseCode.Success;

                }
            }
            return result;
        }

        public async Task<ApiResponse<List<User>>> GetUserListExceptCurrentAsync(string id)
        {
            var users = await _userManager.Users.Where(u => u.Blocked == false && u.Id != id).ToListAsync();

            var result = new ApiResponse<List<User>>(users);
            if (users == null)
            {
                result.Code = Core.Response.ResponseCode.DataNotFound;
                result.Message = "Data could not be found";
                result.IsSuccess = false;
            }
            return result;
        }
    }
}
