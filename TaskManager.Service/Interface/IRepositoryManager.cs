using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Service.Interface
{
    public interface IRepositoryManager
    {
        //IRolesManager RolesManager { get; }
        ITaskRepository Task { get; }
        IUserAuthenticationRepository UserAuthentication { get; }
        Task SaveAsync();
        IUserRoleManager RoleManager { get; }
    }
}
