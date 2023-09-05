using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.DTOs
{
    public class TaskFileDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
