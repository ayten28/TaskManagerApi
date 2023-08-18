using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.DTOs
{
    public class CommentsDto
    {
        public int Id { get; set; }
        public string TaskId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreateUser { get; set; }
        public string CreateUserLogin { get; set; }
        public string CreateUserFullName { get; set; }
    }
}
