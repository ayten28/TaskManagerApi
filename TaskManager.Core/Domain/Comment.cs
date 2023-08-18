using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public string TaskId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreateUser { get; set; }
    }
}
