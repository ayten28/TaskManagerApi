using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreateUser { get; set; }
        public string CreateUserLogin { get; set; }
        public string CreateUserFullName { get; set; }
        public bool IsDone { get; set; }
        public string AssignUser { get; set; }
        public string AssignUserLogin { get; set; }
        public string AssignUserFullName { get; set; }
        public DateTime ResolveDate { get; set; }
    }

    public class TaskCreationDto : TaskAddAndUpdateDto 
    {
        //public int Id { get; set; }
        public string TaskDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreateUser { get; set; }
        public string AssignUser { get; set; }
    }

    public class TaskUpdateDto : TaskAddAndUpdateDto 
    {
        public string? TaskDescription { get; set; }
        public bool? IsDone { get; set; }
        public string? AssignUser { get; set; }
        public DateTime? ResolveDate { get; set; }
    }

    public abstract class TaskAddAndUpdateDto
    {
        [Required(ErrorMessage = "Task name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? TaskName { get; set; }
    }
}
