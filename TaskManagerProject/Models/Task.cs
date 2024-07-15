using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerProject.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public int AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        public TaskStatus Status { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public enum TaskStatus
    {
        NotStarted,
        InProgress,
        Completed
    }
}
