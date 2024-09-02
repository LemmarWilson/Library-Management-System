using Library_Management_System.Repositories;
using Library_Management_System.Models;
using Library_Management_System.Logging;
using Microsoft.Extensions.Logging;

namespace Library_Management_System.Services
{
    /// Service class responsible for managing user-related operations.
    /// This includes user registration, login, logout, deletion, 
    /// and displaying all registered users.
    public class UserService
    {
        // Repository for user data management (Stand in for Database)
        private readonly UserRepository userRepository = UserRepository.Instance;

        // Logger for logging user service activities
        private readonly ILogger<UserService> _logger;

        public UserService()
        {
            // Initialize the logger using a custom logging configuration
            _logger = LoggingConfig.CreateLogger<UserService>();
        }


        /* Registers a new user if the username is not already taken.
        *
        * This method checks if the username exists in the repository.
        * If the username is not found, it creates a new user and adds it to the repository.
        * Logs the result of the registration attempt.
        *
        * @param username The username of the user.
        * @param password The password of the user.
        * @param email The email address of the user.
        * @return True if registration is successful; otherwise, false.
        */
        public bool Register(string username, string password, string email, Role role = Role.USER)
        {
            // Check if the username already exists
            if (userRepository.GetUserByUsername(username) != null)
            {
                // Log a warning if the registration fails due to an existing username
                _logger.LogWarning("Registration failed: Username {Username} already exists.", username);
                return false; // Indicate failed registration
            }

            // Create a new User object with the provided details
            var user = new User(username, password, email, role);

            // Add the new user to the repository
            userRepository.AddUser(user);

            // Log the successful registration
            _logger.LogInformation("User {Username} registered successfully.", username);

            return true; // Indicate successful registration
        }


        /* Logs in a user if the username and password are correct.
        *
        * This method retrieves the user by username and verifies the password.
        * If the credentials are valid, it logs the user in and updates the login status.
        * Logs the result of the login attempt.
        *
        * @param username The username of the user.
        * @param password The password of the user.
        * @return True if login is successful; otherwise, false.
        */
        public bool Login(string username, string password)
        {
            // Retrieve the user by username from the repository
            var user = userRepository.GetUserByUsername(username);

            // Check if the user exists
            if (user == null)
            {
            _logger.LogWarning("Login failed: User with username {Username} not found.", username);
            return false; // Indicate failed login due to non-existent user
            }

            // Validate the password
            if (user.Password != password)
            {
            _logger.LogWarning("Login failed: Invalid password for username {Username}.", username);
            return false; // Indicate failed login due to incorrect password
            }

            // Set the user's login status to true
            user.IsLoggedIn = true;

            // Log the successful login
            _logger.LogInformation("User {Username} logged in successfully.", username);

            return true; // Indicate successful login
        }


        /* Logs out a user if they are currently logged in.
        *
        * This method retrieves the user by username and checks if they are logged in.
        * If the user is logged in, it logs them out and updates the login status.
        * Logs the result of the logout attempt.
        *
        * @param username The username of the user.
        */
        public void Logout(string username)
        {
            // Retrieve the user by username from the repository
            var user = userRepository.GetUserByUsername(username);

            // Check if the user exists
            if (user == null)
            {
                // Log a warning if logout fails because the user does not exist
                _logger.LogWarning("Logout failed: User {Username} not found.", username);
                return; // Exit the method
            }

            // Check if the user is logged in
            if (!user.IsLoggedIn)
            {
                // Log a warning if logout fails because the user is not logged in
                _logger.LogWarning("Logout failed: User {Username} is not logged in.", username);
                return; // Exit the method
            }

            // Set the user's login status to false
            user.IsLoggedIn = false;

            // Log the successful logout
            _logger.LogInformation("User {Username} logged out successfully.", username);
        }


        /* Deletes a user by their username.
        *
        * This method deletes the user from the repository based on their username.
        * Logs the result of the deletion attempt.
        *
        * @param username The username of the user to delete.
        */
        public void DeleteUser(string username)
        {
            // Retrieve the user by username from the repository
            var user = userRepository.GetUserByUsername(username);

            // Check if the user exists
            if (user == null)
            {
                // Log a warning if deletion fails because the user does not exist
                _logger.LogWarning("Deletion failed: User {Username} not found.", username);
                return; // Exit the method
            }

            // Delete the user from the repository
            userRepository.DeleteUser(username);

            // Log the successful deletion
            _logger.LogInformation("User {Username} deleted successfully.", username);
        }

        /* Displays all registered users with their details.
        *
        * This method retrieves all users from the repository and logs their details.
        * Logs the result of the display operation.
        */
        public void DisplayAllUsers()
        {
            // Retrieve all users from the repository
            var users = userRepository.GetAllUsers();

            // Check if there are any users in the repository
            if (users.Count == 0)
            {
                // Log that no users exist
                _logger.LogInformation("No registered users found.");
            }
            else
            {
                // Log information about displaying users
                _logger.LogInformation("Displaying all registered users:");
                
                // Iterate through each user and log their details
                foreach (var user in users)
                {
                    _logger.LogInformation("Username: {Username}, Email: {Email}, Logged In: {IsLoggedIn}", 
                        user.Username, user.Email, user.IsLoggedIn);
                }
            }
        }

        
    }
}