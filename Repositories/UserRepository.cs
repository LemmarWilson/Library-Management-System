using Library_Management_System.Models;

namespace Library_Management_System.Repositories
{
    /**
     * Repository for managing user data.
     *
     * Provides a central place to store, retrieve, update, and delete users within the system.
     * Implements a singleton pattern to ensure a single instance of the repository is used.
     */
    public class UserRepository
    {
        // Dictionary to store users by their username for quick lookup
        private readonly Dictionary<string, User> users = new();

        // Singleton instance
        private static UserRepository? _instance;

        // Private constructor to prevent direct instantiation
        private UserRepository() { }

        /**
         * Gets the singleton instance of the repository.
         *
         * Ensures that only one instance of the repository exists during the program's lifecycle.
         *
         * @return The singleton instance of UserRepository.
         */
        public static UserRepository Instance => _instance ??= new UserRepository();

        /**
         * Adds a new user to the repository.
         *
         * This method adds the provided user object to the dictionary.
         *
         * @param user The User object to be added to the repository.
         */
        public void AddUser(User user)
        {
            users[user.Username] = user;
        }

        /**
         * Retrieves a user by their username.
         *
         * Searches the dictionary for a user with the specified username.
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

        /**
         * Deletes a user by their username.
         *
         * Searches for a user with the specified username and removes them from the dictionary if found.
         *
         * @param username The username of the user to delete.
         */
        public void DeleteUser(string username)
        {
            users.Remove(username);
        }

        /**
         * Retrieves all users from the repository.
         *
         * Returns all users currently stored in the dictionary.
         *
         * @return A list of all User objects in the repository.
         */
        public List<User> GetAllUsers()
        {
            return new List<User>(users.Values);
        }

        /**
         * Checks if a user exists and is logged in.
         *
         * This method verifies if a user with the specified username exists in the repository
         * and is currently logged in.
         *
         * @param username The username of the user to check.
         * @return True if the user exists and is logged in; otherwise, false.
         */
        public bool IsUserLoggedIn(string username)
        {
            var user = GetUserByUsername(username);
            return user != null && user.IsLoggedIn;
        }

        /**
         * Updates an existing user's information in the repository.
         *
         * This method updates a user's details in the repository. If the username is changing,
         * the old entry is removed, and the updated user is added with the new username as the key.
         *
         * @param currentUsername The current username of the user to update.
         * @param updatedUser The updated User object containing the new details.
         * @throws ArgumentException if the user with the current username does not exist.
         */
        public void UpdateUser(string currentUsername, User updatedUser)
        {
            // Ensure the current username exists in the dictionary
            string currentKey = currentUsername;
            if (!users.ContainsKey(currentKey))
            {
                throw new ArgumentException("User not found in repository.");
            }

            // Remove the old entry if the username is changing
            users.Remove(currentKey);

            // Add the updated user with the new username as the key
            string newUsername = updatedUser.Username;
            users[newUsername] = updatedUser;
        }
    }
}
