using Microsoft.AspNetCore.Identity;

namespace TaskManager.Core.Domain
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public string? Gender { get; set; }
        public bool Blocked { get; set; }        
        public string? PhotoUrl { get; set; }
    }
}
