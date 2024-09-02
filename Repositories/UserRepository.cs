using Library_Management_System.Models;

namespace Library_Management_System.Repositories
{
    public class UserRepository
    {
        // Dictionary to store users by their username for quick lookup
        private readonly Dictionary<string, User> users = [];

        // Singleton instance
        private static UserRepository? _instance;

        // Private constructor to prevent direct instantiation
        private UserRepository() { }

        // Public method to get the singleton instance
        public static UserRepository Instance => _instance ??= new UserRepository();


        /* Adds a new user to the repository.
        *
        * This method adds the provided user object to the dictionary.
        * It assumes that the caller has already performed any necessary validations.
        *
        * @param user The User object to be added to the repository.
        */
        public void AddUser(User user)
        {
            // Add the user to the Dictionary
            users[user.Username] = user;
        }

        /* Retrieves a user by their username.
        *
        * This method searches the dictionary for a user with the specified username.
        * If found, the user is returned; otherwise, null is returned.
        *
        * @param username The username of the user to retrieve.
        * @return The User object if found; otherwise, null.
        */
        public User? GetUserByUsername(string username)
        {
            users.TryGetValue(username, out var user);
            return user;
        }

        /* Deletes a user by their username.
        *
        * This method searches for a user with the specified username and removes them 
        * from the dictionary if found.
        *
        * @param username The username of the user to delete.
        */
        public void DeleteUser(string username)
        {
            users.Remove(username);
        }

        /* Retrieves all users from the repository.
        *
        * This method returns all users stored in the dictionary.
        *
        * @return A list of all User objects in the repository.
        */
        public List<User> GetAllUsers()
        {
            return new List<User>(users.Values);
        }


        /* Checks if a user exists and is logged in.
        *
        * This method checks if a user with the specified username exists and if they are currently logged in.
        *
        * @param username The username of the user to check.
        * @return True if the user exists and is logged in; otherwise, false.
        */
        public bool IsUserLoggedIn(string username)
        {
            var user = GetUserByUsername(username);
            return user != null && user.IsLoggedIn;
        }

        
    }
}
