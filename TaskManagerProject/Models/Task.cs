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
        public required string Title { get; set; }

        [StringLength(1000)]
        public required string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public int AssignedUserId { get; set; }
        public required User AssignedUser { get; set; }

        public TaskStatus Status { get; set; }

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
