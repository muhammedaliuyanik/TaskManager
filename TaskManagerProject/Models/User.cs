namespace TaskManagerProject.Models
{
    public enum UserRole
    {
        Admin, // Yönetici
        ProjectManager, // Proje Yöneticisi
        Employee // Çalışan
    }

    public class User
    {
        public int UserId { get; set; } // Kullanıcı ID'si
        public string Username { get; set; } = string.Empty; // Kullanıcı adı
        public string Password { get; set; } = string.Empty; // Kullanıcı şifresi (hashlenmiş)
        public string Email { get; set; } = string.Empty; // Kullanıcı emaili
        public string FirstName { get; set; } = string.Empty; // Kullanıcı adı
        public string LastName { get; set; } = string.Empty; // Kullanıcı soyadı
        public DateTime CreatedDate { get; set; } // Oluşturulma tarihi
        public DateTime UpdatedDate { get; set; } // Güncellenme tarihi
        public UserRole Role { get; set; } = UserRole.Employee; // Kullanıcı rolü (varsayılan: Employee)
    }
}
