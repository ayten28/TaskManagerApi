using Microsoft.AspNetCore.Identity;
using TaskManager.Core.Domain;

namespace TaskManager.Service.Interface
{
    public interface IUserRoleManager
    {
        Task<IdentityResult> CreateRole(string roleName);
        Task<IdentityResult> CreateRoleV2(Role role);
        System.Threading.Tasks.Task DeleteRole(string roleId);
        Task<List<Role>> GetRoleListAsync();
        Task<IdentityResult> AssignRole(string userId, string roleName);
        Task<List<string>> GetUserRoles(string userId);
        Task<IdentityResult> DeleteRole(string userId, string roleName);

    }
}
