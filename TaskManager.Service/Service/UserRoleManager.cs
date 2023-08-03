using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class UserRoleManager : IUserRoleManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private User? _user;
        RoleManager<Role> _roleManager;
        public UserRoleManager(
        UserManager<User> userManager, Microsoft.Extensions.Configuration.IConfiguration configuration, IMapper mapper, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _roleManager = roleManager;
        }


        public async System.Threading.Tasks.Task DeleteRole(string roleId)
        {
            Role role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }               
        }

        public async Task<List<Role>> GetRoleListAsync()
        {           
            return _roleManager.Roles.ToList();
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> CreateRole(string roleName)
        {
            IdentityResult result = await _roleManager.CreateAsync(new Role {Id = Guid.NewGuid().ToString(), Name = roleName });
           
            return result;
        }

        public async Task<IdentityResult> CreateRoleV2(Role role)
        {
            IdentityResult result = await _roleManager.CreateAsync(role);
            return result;
        }

        public async Task<IdentityResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result =  await _userManager.AddToRoleAsync(user, roleName);
            return result;           
        }

        public async Task<List<string>> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<IdentityResult> DeleteRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByNameAsync(roleName);

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result;
        }
    }
}
