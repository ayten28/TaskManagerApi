using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.DTOs
{
    public class AddCommentDto
    {
        public string TaskId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreateUser { get; set; }
    }
}
