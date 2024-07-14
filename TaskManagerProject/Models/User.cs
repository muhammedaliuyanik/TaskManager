using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerProject.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Username { get; set; }

        [Required]
        [StringLength(100)]
        public required string Password { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public enum UserRole
    {
        Admin,
        ProjectManager,
        Employee
    }
}
