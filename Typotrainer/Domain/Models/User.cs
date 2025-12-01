namespace Domain.Models
{
    /// <summary>
    /// User entity - represents a registered user
    /// </summary>
    public class User
    {
        public int Id { get; private set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        // Empty constructor for repository
        public User() { }

        // Constructor for creating new user
        public User(string email, string passwordHash)
        {
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}
