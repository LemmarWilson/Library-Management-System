using Library_Management_System.Repositories;
using Library_Management_System.Models;
using Library_Management_System.Logging;
using Microsoft.Extensions.Logging;

namespace Library_Management_System.Services
{
    /* 
    * Service class responsible for managing user-related operations.
    * This includes user registration, login, logout, deletion, and more.
    */
    public class UserService
    {
        // Repository for user data management (stand-in for a database)
        private readonly UserRepository userRepository = UserRepository.Instance;

        // Logger for tracking and recording user service activities
        private readonly ILogger<UserService> _logger;

        /// Constructor to initialize the user service and its logger.
        public UserService()
        {
            // Initialize the logger using a custom logging configuration
            _logger = LoggingConfig.CreateLogger<UserService>();
        }


        /* 
        * Registers a new user if the username is not already taken.
        *
        * This method checks if the username exists in the repository. If the username is not
        * found, it creates a new user and adds it to the repository. Logs the result of the
        * registration attempt.
        *
        * @param username The username of the user.
        * @param password The password of the user.
        * @param email The email address of the user.
        * @param role The role of the user (defaults to USER).
        * @return True if registration is successful; otherwise, throws an exception.
        */
        public bool Register(string username, string password, string email, Role role = Role.USER)
        {
            // Check if the username already exists
            if (userRepository.GetUserByUsername(username) != null)
            {
                _logger.LogWarning("Registration failed: Username {Username} already exists.", username);
                throw new ArgumentException($"Username '{username}' already exists.");
            }

            // Validate input fields
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Registration failed: Invalid input.");
                throw new ArgumentException("Invalid input. Username, password, and email are required.");
            }

            // Validate email format
            if (!IsValidEmail(email))
            {
                _logger.LogWarning("Registration failed: Invalid email format.");
                throw new ArgumentException("Invalid email format.");
            }

            // Create a new User object
            var user = new User(username, email, role);

            try
            {
                // Validate and set the password
                user.Password.SetPassword(password);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Password validation failed: {ErrorMessage}", ex.Message);
                throw; // Re-throw the exception to be handled at a higher level
            }

            try
            {
                // Input security questions
                user.SecurityQuestions.InputSecurityQuestions();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to set security questions: {ErrorMessage}", ex.Message);
                throw new Exception("Failed to set security questions. Please try again.");
            }

            // Add the user to the repository
            userRepository.AddUser(user);

            _logger.LogInformation("User {Username} registered successfully.", username);
            return true;
        }

        /* 
        * Logs in a user if the username and password are correct.
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
            // Check for empty or null input
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Login failed: Username and password must not be empty.");
                Console.WriteLine("Username and password are required.");
                return false; // Indicate failed login
            }

            // Retrieve the user by username from the repository
            var user = userRepository.GetUserByUsername(username);

            // Check if the user exists
            if (user == null)
            {
                _logger.LogWarning("Login failed: User with username {Username} not found.", username);
                Console.WriteLine("User not found. Please check your username.");
                return false; // Indicate failed login due to non-existent user
            }

            // Validate the password
            if (!user.Password.VerifyPassword(password)) // Note the negation
            {
                _logger.LogWarning("Login failed: Invalid password for username {Username}.", username);
                Console.WriteLine("Invalid password. Please try again.");
                return false; // Indicate failed login due to incorrect password
            }

            // Check if the user is already logged in
            if (user.IsLoggedIn)
            {
                _logger.LogWarning("Login failed: User {Username} is already logged in.", username);
                Console.WriteLine("User is already logged in.");
                return false; // Indicate failed login due to user already being logged in
            }

            // Set the user's login status to true
            user.IsLoggedIn = true;

            // Log the successful login
            _logger.LogInformation("User {Username} logged in successfully.", username);
            Console.WriteLine($"Welcome, {username}! You are now logged in.");

            return true; // Indicate successful login
        }



        /* 
        * Logs out a user if they are currently logged in.
        *
        * This method retrieves the user by username and checks if they are logged in.
        * If the user is logged in, it logs them out and updates their login status.
        * Logs the result of the logout attempt and provides feedback to the user.
        *
        * @param username The username of the user attempting to log out.
        */
        public void Logout(string username)
        {
            // Check for empty or null input
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogWarning("Logout failed: Username must not be empty.");
                Console.WriteLine("Logout failed: Username is required.");
                return; // Exit the method
            }

            // Retrieve the user by username from the repository
            var user = userRepository.GetUserByUsername(username);

            // Check if the user exists
            if (user == null)
            {
                _logger.LogWarning("Logout failed: User {Username} not found.", username);
                Console.WriteLine("Logout failed: User not found.");
                return; // Exit the method
            }

            // Check if the user is logged in
            if (!user.IsLoggedIn)
            {
                _logger.LogWarning("Logout failed: User {Username} is not logged in.", username);
                Console.WriteLine("Logout failed: User is not logged in.");
                return; // Exit the method
            }

            // Set the user's login status to false
            user.IsLoggedIn = false;

            // Log the successful logout
            _logger.LogInformation("User {Username} logged out successfully.", username);
            Console.WriteLine($"User {username} has been logged out successfully.");
        }

        /* 
        * Deletes a user by their username.
        *
        * This method removes the user from the repository based on their username.
        * A confirmation prompt is provided to prevent accidental deletions.
        * Logs the result of the deletion attempt and provides feedback to the user.
        *
        * @param username The username of the user to delete.
        */
        public void DeleteUser(string username)
        {
            // Check for empty or null input
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogWarning("Deletion failed: Username must not be empty.");
                Console.WriteLine("Deletion failed: Username is required.");
                return; // Exit the method
            }

            // Retrieve the user by username from the repository
            var user = userRepository.GetUserByUsername(username);

            // Check if the user exists
            if (user == null)
            {
                _logger.LogWarning("Deletion failed: User {Username} not found.", username);
                Console.WriteLine("Deletion failed: User not found.");
                return; // Exit the method
            }

            // Confirm the deletion with the user
            Console.Write($"Are you sure you want to delete user '{username}'? This action cannot be undone. (y/n): ");
            var confirmation = Console.ReadLine();
            if (confirmation?.ToLower() != "y")
            {
                Console.WriteLine("Deletion cancelled.");
                return; // Exit the method
            }

            // Delete the user from the repository
            userRepository.DeleteUser(username);

            // Log the successful deletion
            _logger.LogInformation("User {Username} deleted successfully.", username);
            Console.WriteLine($"User '{username}' has been deleted successfully.");
        }



        /* 
        * Displays all registered users with their details.
        *
        * This method retrieves all users from the repository and logs their details.
        * Access is restricted to staff or admin users only.
        * Logs the result of the display operation, including access attempts.
        *
        * @param adminUsername The username of the user attempting to display all users.
        */
        public void DisplayAllUsers(string adminUsername)
        {
            // Check if the provided user is an admin or staff
            if (!UserIsStaff(adminUsername))
            {
                _logger.LogWarning("Access denied: User {Username} attempted to display all users without sufficient permissions.", adminUsername);
                Console.WriteLine("Access denied: You do not have permission to view all users.");
                return; // Exit the method
            }

            // Retrieve all users from the repository
            var users = userRepository.GetAllUsers();

            // Check if there are any users in the repository
            if (users == null || users.Count == 0)
            {
                // Log that no users exist
                _logger.LogInformation("No registered users found.");
                Console.WriteLine("No registered users found.");
            }
            else
            {
                // Log information about displaying users
                _logger.LogInformation("Displaying all registered users:");

                // Display each user's details
                Console.WriteLine("Registered Users:");
                foreach (var user in users)
                {
                    Console.WriteLine($"Username: {user.Username}, Email: {user.Email}, Logged In: {user.IsLoggedIn}");
                }
            }
        }

        /* 
        * Changes the password for a user.
        *
        * This method retrieves the user by their username and verifies their old password.
        * If the verification passes, it updates the password and logs the user out.
        * Logs the result of the password change attempt.
        *
        * @param username The username of the user changing their password.
        * @param oldPassword The current password of the user.
        * @param newPassword The new password to set for the user.
        * @throws ArgumentException If the user does not exist or the old password is incorrect.
        * @throws InvalidOperationException If the user is not logged in.
        */
        public void ChangePassword(string username, string oldPassword, string newPassword)
        {
            // Retrieve the user by username
            var user = userRepository.GetUserByUsername(username);

            // Check if the user exists
            if (user == null)
            {
                _logger.LogWarning("Password change failed: User {Username} not found.", username);
                throw new ArgumentException($"User '{username}' does not exist.");
            }

            // Check if the user is logged in
            if (!user.IsLoggedIn)
            {
                _logger.LogWarning("Password change failed: User {Username} is not logged in.", username);
                throw new InvalidOperationException("You must be logged in to change your password.");
            }

            // Verify the old password
            if (!user.Password.VerifyPassword(oldPassword))
            {
                _logger.LogWarning("Password change failed: Invalid old password for user {Username}.", username);
                throw new ArgumentException("Old password is incorrect.");
            }

            try
            {
                // Set the new password
                user.Password.SetPassword(newPassword);

                // Log the user out
                user.IsLoggedIn = false;
                _logger.LogInformation("Password changed successfully for user {Username}. User logged out.", username);
                Console.WriteLine("Password changed successfully. You have been logged out. Please log in with your new password.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Password change failed: {ErrorMessage}", ex.Message);
                throw new ArgumentException($"Password change failed: {ex.Message}");
            }
        }


        /* 
        * Allows a user to reset their password by answering security questions.
        *
        * This method retrieves the user by their username and presents a set of randomly 
        * selected security questions. The user must answer the questions correctly to reset 
        * their password. Logs the result of the password reset attempt.
        *
        * @param username The username of the user attempting to reset their password.
        * @throws ArgumentException If the user does not exist or answers the security questions incorrectly.
        */
        public void ForgotPassword(string username)
        {
            // Retrieve the user by username
            var user = userRepository.GetUserByUsername(username);

            // Check if the user exists
            if (user == null)
            {
                _logger.LogWarning("Password reset failed: User {Username} not found.", username);
                throw new ArgumentException($"User '{username}' does not exist.");
            }

            Console.WriteLine("Answer the following security questions to verify your identity:");

            // Retrieve security questions
            var securityQuestions = user.SecurityQuestions.GetSecurityQuestions();

            // Randomly select three questions
            var random = new Random();
            var selectedQuestions = securityQuestions
                .OrderBy(x => random.Next())
                .Take(3)
                .ToList();

            // Verify answers
            foreach (var question in selectedQuestions)
            {
                Console.WriteLine(question.Key);
                string? answer = Console.ReadLine()?.ToUpper().Trim();

                if (answer != question.Value)
                {
                    _logger.LogWarning("Password reset failed: Incorrect answer for security question for user {Username}.", username);
                    throw new ArgumentException("Incorrect answers to security questions.");
                }
            }

            // Allow the user to set a new password
            try
            {
                Console.Write("Enter your new password: ");
                string newPassword = Console.ReadLine();
                user.Password.SetPassword(newPassword);

                _logger.LogInformation("Password reset successful for user {Username}.", username);
                Console.WriteLine("Password has been reset successfully. Please log in with your new password.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Password reset failed: {ErrorMessage}", ex.Message);
                throw new ArgumentException($"Password reset failed: {ex.Message}");
            }
        }

        /* 
        * Updates the username and email for a user.
        *
        * This method retrieves the user by their current username and validates the provided 
        * new username and email. If the inputs are valid, it updates the user profile and logs 
        * the user out to enforce a re-login. Logs the result of the update attempt.
        *
        * @param username The current username of the user.
        * @param newUsername The new username to set for the user.
        * @param newEmail The new email address to set for the user.
        * @throws ArgumentException If the user does not exist, the inputs are invalid, or the new username already exists.
        * @throws InvalidOperationException If the user is not logged in.
        */
        public void UpdateUser(string username, string newUsername, string newEmail)
        {
            // Retrieve the user by username
            var user = userRepository.GetUserByUsername(username);

            // Check if the user exists
            if (user == null)
            {
                _logger.LogWarning("Update failed: User {Username} not found.", username);
                throw new ArgumentException($"User '{username}' does not exist.");
            }

            // Check if the user is logged in
            if (!user.IsLoggedIn)
            {
                _logger.LogWarning("Update failed: User {Username} is not logged in.", username);
                throw new InvalidOperationException("You must be logged in to update your profile.");
            }

            // Validate new inputs
            if (string.IsNullOrWhiteSpace(newUsername) || string.IsNullOrWhiteSpace(newEmail))
            {
                _logger.LogWarning("Update failed: Invalid input for user {Username}.", username);
                throw new ArgumentException("Username and email cannot be empty.");
            }

            // Validate email format
            if (!IsValidEmail(newEmail))
            {
                _logger.LogWarning("Update failed: Invalid email format for user {Username}.", username);
                throw new ArgumentException("Invalid email format.");
            }

            // Check if the new username already exists
            if (!username.Equals(newUsername, StringComparison.OrdinalIgnoreCase) &&
                userRepository.GetUserByUsername(newUsername) != null)
            {
                _logger.LogWarning("Update failed: New username {NewUsername} already exists.", newUsername);
                throw new ArgumentException($"Username '{newUsername}' is already taken.");
            }

            // Update user details
            user.Username = newUsername;
            user.Email = newEmail;
            userRepository.UpdateUser(username, user);

            // Log the user out
            user.IsLoggedIn = false;

            _logger.LogInformation("User {OldUsername} updated successfully to {NewUsername}.", username, newUsername);
            Console.WriteLine($"User '{username}' updated successfully to username '{newUsername}' and email '{newEmail}'.");
        }


/* 
        ====================================================================================================
                                                HELPERS
        ====================================================================================================
*/

        /* 
        * Checks if a user has staff or admin permissions.
        *
        * This method retrieves the user by their username and checks their role.
        * Only users with the ADMIN or STAFF role have the necessary permissions for certain actions.
        * Logs the result of the permission check.
        *
        * @param username The username of the user to check.
        * @return True if the user has staff or admin permissions; otherwise, false.
        */
        private bool UserIsStaff(string username)
        {
            // Retrieve the user by username
            var user = userRepository.GetUserByUsername(username);

            // Check if the user exists
            if (user == null)
            {
                _logger.LogWarning("Permission check failed: User {Username} does not exist.", username);
                Console.WriteLine($"Error: User '{username}' does not exist.");
                return false; // Exit if the user does not exist
            }

            // Check if the user has the STAFF or ADMIN role
            if (user.Role != Role.ADMIN && user.Role != Role.STAFF)
            {
                _logger.LogWarning("Permission denied: User {Username} does not have sufficient permissions.", username);
                Console.WriteLine($"Error: User '{username}' does not have permission for this action.");
                return false; // Exit if the user does not have the required role
            }

            return true; // User has the necessary permissions
        }

        /* 
        * Validates the format of an email address.
        *
        * This method uses the `System.Net.Mail.MailAddress` class to check 
        * if the provided email is in a valid format. Returns true for a valid email.
        *
        * @param email The email address to validate.
        * @return True if the email is valid; otherwise, false.
        */
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false; // Return false if the email format is invalid
            }
        }

    }
}