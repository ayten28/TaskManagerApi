using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.DTOs
{
    public class UserUpdateDto
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? PhoneNumber { get; set; }
        public string? Position { get; set; }
        public string? Gender { get; set; }
    }
}
