using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskManager.Core.Domain
{
    public class UserRole : IdentityUserRole
    {
        public override string RoleId { get; set; }
        public override string UserId { get; set; }
    }
}
