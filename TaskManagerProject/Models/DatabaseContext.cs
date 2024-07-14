using Microsoft.EntityFrameworkCore;

namespace TaskManagerProject.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!; // Users tablosu
        public DbSet<Task> Tasks { get; set; } = null!; // Tasks tablosu

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
