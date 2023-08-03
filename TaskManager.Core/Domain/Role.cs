using Microsoft.AspNetCore.Identity;

namespace TaskManager.Core.Domain
{
    public class Role : IdentityRole
    {
        public override string  Id { get; set; }
        public override string Name { get; set; }
        public override string? NormalizedName { get; set; }
        public override string? ConcurrencyStamp { get; set; }
    }
}
