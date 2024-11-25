using System.Globalization;
using System.Threading.Channels;

namespace Library_Management_System.Models
{
    public class User
    {
        public string Username { get; private set; }
        //public string Password { get; private set; } //might break stuff

        public Password Password { get; private set; } //Amber's
        public string Email { get; private set; }
        public bool IsLoggedIn { get; set; } = false;
        public Role Role { get; private set; }
        public SecurityQuestions SecurityQuestions { get; private set; } //Added property for user to access security questions

        // Constructor that accepts all properties, including role
        public User(string username, string email, Role role) 
        {
            Username = username;
            //Password = password;
            Password = new Password(); //Amber's
            Email = email;
            Role = role;
            SecurityQuestions = new SecurityQuestions(); //Amber's
        }
    }
}
