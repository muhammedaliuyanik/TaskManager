namespace TaskManagerProject.Models
{
    public class UserDto
    {
        public int UserId { get; set; } // Kullanıcı ID'si
        public string Username { get; set; } = string.Empty; // Kullanıcı adı
        public string Password { get; set; } = string.Empty; // Kullanıcı şifresi
        public string Email { get; set; } = string.Empty; // Kullanıcı emaili
        public string FirstName { get; set; } = string.Empty; // Kullanıcı adı
        public string LastName { get; set; } = string.Empty; // Kullanıcı soyadı
    }
}
