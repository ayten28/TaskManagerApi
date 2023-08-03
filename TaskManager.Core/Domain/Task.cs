using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Domain
{
    public class Task
    {
        [Column("TaskId")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Task name is a required field.")]
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreateUser { get; set; }
        public bool IsDone { get; set; }
        public string AssignUser { get; set; }
        public DateTime ResolveDate { get; set; }

    }
}
