namespace Library_Management_System.Models
{
    public class User
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public bool IsLoggedIn { get; set; } = false;
        public Role Role { get; private set; }

        // Constructor that accepts all properties, including role
        public User(string username, string password, string email, Role role)
        {
            Username = username;
            Password = password;
            Email = email;
            Role = role;
        }
    }
}
