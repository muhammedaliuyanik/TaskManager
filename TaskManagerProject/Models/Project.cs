using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerProject.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty; // Title alanını ekliyoruz

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
