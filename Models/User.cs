using System.Globalization;
using System.Threading.Channels;

namespace Library_Management_System.Models
{
    public class User
    {
        public string Username { get; set; }

        public Password Password { get; private set; }
        public string Email { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        public Role Role { get; private set; }
        public SecurityQuestions SecurityQuestions { get; private set; }

        // Constructor that accepts all properties, including role
        public User(string username, string email, Role role) 
        {
            Username = username;
            //Password = password;
            Password = new Password();
            Email = email;
            Role = role;
            SecurityQuestions = new SecurityQuestions();
        }
    }
}
