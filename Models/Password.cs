using System.Security.Cryptography;
using System.Text;

namespace Library_Management_System.Models
{
    /**
     * Represents a secure password system that provides functionality for
     * verifying passwords, hashing them securely, and enforcing complexity rules.
     */
    public class Password
    {
        // Stores the hashed version of the password
        private string _hashedPassword;

        /**
         * Verifies whether the provided password matches the stored hashed password.
         *
         * This method hashes the provided password and compares it to the stored hash.
         *
         * @param checkPassword The password to verify.
         * @return True if the password matches the stored hash; otherwise, false.
         * @throws ArgumentException if the provided password is null or empty.
         */
        public bool VerifyPassword(string checkPassword)
        {
            if (string.IsNullOrWhiteSpace(checkPassword))
                throw new ArgumentException("Password cannot be null or empty.");

            string hashedCheckPassword = HashPassword(checkPassword.Trim());
            return _hashedPassword == hashedCheckPassword;
        }

        /**
         * Retrieves the hashed version of the password.
         *
         * This method returns the stored hashed password, which is securely generated
         * using SHA-256.
         *
         * @return The hashed password as a string.
         */
        public string GetHashedPassword()
        {
            return _hashedPassword;
        }

        /**
         * Sets the password after validating its complexity and hashing it securely.
         *
         * This method enforces password complexity rules and stores the hashed version
         * of the password.
         *
         * @param newPassword The password to set.
         * @throws ArgumentException if the password is null, empty, or does not meet
         *         complexity requirements.
         */
        public void SetPassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Password cannot be null or empty.");

            newPassword = newPassword.Trim();

            // Validate the password's complexity
            ValidatePasswordComplexity(newPassword);

            // Hash the password and store it
            _hashedPassword = HashPassword(newPassword);
        }

        /**
         * Hashes the provided password using SHA-256.
         *
         * This method securely hashes the input password and returns the resulting hash
         * as a hexadecimal string.
         *
         * @param password The password to hash.
         * @return The hashed password as a hexadecimal string.
         */
        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }

                return hashString.ToString();
            }
        }

        /**
         * Validates the complexity of the provided password.
         *
         * This method enforces specific rules, such as minimum and maximum length,
         * and requires at least one uppercase letter, one lowercase letter, one digit,
         * and one special character.
         *
         * @param password The password to validate.
         * @throws ArgumentException if the password does not meet complexity requirements.
         */
        private static void ValidatePasswordComplexity(string password)
        {
            const int MinLength = 6;
            const int MaxLength = 15;
            const string SpecialCharacters = "!@#$%^&*:;.?<>_-+=";

            if (password.Length < MinLength || password.Length > MaxLength)
                throw new ArgumentException($"Password must be between {MinLength} and {MaxLength} characters.");

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (SpecialCharacters.Contains(c)) hasSpecial = true;
            }

            if (!hasUpper)
                throw new ArgumentException("Password must include at least one uppercase letter.");
            if (!hasLower)
                throw new ArgumentException("Password must include at least one lowercase letter.");
            if (!hasDigit)
                throw new ArgumentException("Password must include at least one number.");
            if (!hasSpecial)
                throw new ArgumentException("Password must include at least one special character.");
        }
    }
}
