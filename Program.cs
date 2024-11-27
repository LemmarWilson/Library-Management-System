using Library_Management_System.Services;
using Library_Management_System.Models;
using Library_Management_System.Repositories;
using System;

namespace Library_Management_System
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Library Management System!");

            var userService = new UserService();
            var bookService = new BookService();

            bookService.PreAddBooks();

            string? currentUser = null;

            while (true)
            {
                DisplayMenu();
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    // User Management
                    case "1":
                        RegisterUser(userService);
                        break;
                    case "2":
                        RegisterAdmin(userService);
                        break;
                    case "3":
                        currentUser = LogInUser(userService);
                        break;
                    case "4":
                        LogOutUser(userService, ref currentUser);
                        break;
                    case "5":
                        UpdateUserProfile(userService, currentUser);
                        break;
                    case "6":
                        ChangeUserPassword(userService, currentUser);
                        break;
                    case "7":
                        ForgotPassword(userService);
                        break;
                    case "8":
                        DeleteUser(userService, currentUser);
                        break;

                    // Library Card Management
                    case "9":
                        RegisterLibraryCard(currentUser);
                        break;
                    case "10":
                        RenewCardEarly(currentUser);
                        break;
                    case "11":
                        DisplayLibraryCard(currentUser);
                        break;
                    case "12":
                        DisplayBorrowedBooks(currentUser);
                        break;

                    // Book Management
                    case "13":
                        AddBook(bookService, currentUser);
                        break;
                    case "14":
                        BorrowBook(bookService, currentUser);
                        break;
                    case "15":
                        ReturnBook(bookService, currentUser);
                        break;
                    case "16":
                        ReserveBook(bookService, currentUser);
                        break;
                    case "17":
                        CancelReservation(bookService, currentUser);
                        break;
                    case "18":
                        DisplayAllBooks(bookService, currentUser);
                        break;
                    case "19":
                        DisplayAvailableBooks(bookService, currentUser);
                        break;
                    case "20":
                        SearchBookByTitle(bookService, currentUser);
                        break;
                    case "21":
                        SearchBookByAuthor(bookService, currentUser);
                        break;
                    case "22":
                        UpdateBookDetails(bookService, currentUser);
                        break;
                    case "23":
                        DeleteBook(bookService, currentUser);
                        break;

                    // Exit
                    case "24":
                        Console.WriteLine("Exiting the program...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        /* Displays the main menu options to the user.
        *
        * This method categorizes the options into User Management, Library Card Management, 
        * and Book Management. It provides users with numbered choices to interact with the system.
        */
        static void DisplayMenu()
        {
            Console.WriteLine("\nPlease select an option:");

            // User Management
            Console.WriteLine("1.  Register User");
            Console.WriteLine("2.  Register Admin");
            Console.WriteLine("3.  Log In");
            Console.WriteLine("4.  Log Out");
            Console.WriteLine("5.  Update User Email/Username");
            Console.WriteLine("6.  Change Password");
            Console.WriteLine("7.  Forgot Password");
            Console.WriteLine("8.  Delete User");

            // Library Card Management
            Console.WriteLine("9.  Register Library Card");
            Console.WriteLine("10. Renew Library Card Early");
            Console.WriteLine("11. Display Library Card");
            Console.WriteLine("12. Display Borrowed Books");

            // Book Management
            Console.WriteLine("13. Add Book");
            Console.WriteLine("14. Borrow Book");
            Console.WriteLine("15. Return Book");
            Console.WriteLine("16. Reserve Book");
            Console.WriteLine("17. Cancel Reservation");
            Console.WriteLine("18. Display All Books");
            Console.WriteLine("19. Display Available Books");
            Console.WriteLine("20. Search Book by Title");
            Console.WriteLine("21. Search Book by Author");
            Console.WriteLine("22. Update Book");
            Console.WriteLine("23. Delete Book");

            // Exit
            Console.WriteLine("24. Exit");
        }



        /* Registers a new user.
        *
        * This method collects the user's username, password, and email input,
        * then attempts to register the user through the UserService. 
        * Any exceptions encountered during the registration process are caught and displayed.
        *
        * @param userService The UserService instance used to register the user.
        */
        static void RegisterUser(UserService userService)
        {
            try
            {
                var (username, password, email) = GetUserInput();
                userService.Register(username, password, email);
                Console.WriteLine($"User '{username}' registered successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Registration failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /* Registers a new admin user.
        *
        * This method collects the admin's username, password, and email input,
        * then attempts to register the admin through the UserService.
        * A success or failure message is displayed based on the result.
        *
        * @param userService The UserService instance used to register the admin.
        */
        static void RegisterAdmin(UserService userService)
        {
            var (username, password, email) = GetUserInput();
            if (userService.Register(username, password, email, Role.ADMIN))
                Console.WriteLine($"Admin '{username}' registered successfully.");
            else
                Console.WriteLine($"Registration failed: Username '{username}' already exists.");
        }

        /* Logs in a user.
        *
        * This method prompts the user to enter their username and password,
        * then attempts to log the user in through the UserService.
        * On successful login, the username is returned; otherwise, null is returned.
        *
        * @param userService The UserService instance used to log in the user.
        * @return The username of the logged-in user, or null if login fails.
        */
        static string LogInUser(UserService userService)
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            if (userService.Login(username, password))
            {
                Console.WriteLine($"User '{username}' logged in successfully.");
                return username;
            }
            else
            {
                Console.WriteLine("Login failed. Check your credentials.");
                return null;
            }
        }


        /* Logs out the currently logged-in user.
        *
        * This method logs out the user specified by the `currentUser` variable
        * by calling the `Logout` method in the `UserService`. If no user is
        * logged in, it displays an appropriate message. On successful logout,
        * the `currentUser` variable is reset to null.
        *
        * @param userService The UserService instance used to log out the user.
        * @param currentUser A reference to the currently logged-in user's username.
        */
        static void LogOutUser(UserService userService, ref string currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("No user is currently logged in.");
                return;
            }

            userService.Logout(currentUser);
            Console.WriteLine($"User '{currentUser}' logged out successfully.");
            currentUser = null;
        }

        /* Updates the profile of the currently logged-in user.
        *
        * This method allows the logged-in user to update their username and email.
        * If the user chooses not to change a field, the current value is retained.
        * The method validates input and updates the user profile through the `UserService`.
        * If the username changes, the `currentUser` variable is updated accordingly.
        * Errors encountered during the update process are caught and displayed.
        *
        * @param userService The UserService instance used to update the user profile.
        * @param currentUser The username of the currently logged-in user.
        */
        static void UpdateUserProfile(UserService userService, string currentUser)
        {
            // Ensure the user is logged in
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to update your profile.");
                return;
            }

            try
            {
                Console.Write("Enter new username (or press Enter to keep unchanged): ");
                string newUsername = Console.ReadLine();
                newUsername = string.IsNullOrWhiteSpace(newUsername) ? currentUser : newUsername;

                Console.Write("Enter new email (or press Enter to keep unchanged): ");
                string newEmail = Console.ReadLine();
                UserRepository userRepository = UserRepository.Instance;
                newEmail = string.IsNullOrWhiteSpace(newEmail) ? userRepository.GetUserByUsername(currentUser).Email : newEmail;

                // Call the UpdateUser method
                userService.UpdateUser(currentUser, newUsername, newEmail);

                // Update the currentUser variable if the username changes
                if (!newUsername.Equals(currentUser, StringComparison.OrdinalIgnoreCase))
                {
                    currentUser = newUsername;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }


        /* Changes the password for the currently logged-in user.
        *
        * This method prompts the user to enter their current password and a new password.
        * If the current password matches, the user's password is updated, and they are logged out.
        * Errors during the process are handled and displayed to the user.
        *
        * @param userService The UserService instance used to change the user's password.
        * @param currentUser The username of the currently logged-in user.
        */
        static void ChangeUserPassword(UserService userService, string currentUser)
        {
            // Ensure the user is logged in
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to change your password.");
                return;
            }

            try
            {
                Console.Write("Enter your current password: ");
                string oldPassword = Console.ReadLine();

                Console.Write("Enter your new password: ");
                string newPassword = Console.ReadLine();

                // Call the ChangePassword method
                userService.ChangePassword(currentUser, oldPassword, newPassword);

                // Update the current user to null since they are logged out
                currentUser = null;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /* Initiates the process to reset the user's password.
        *
        * This method prompts the user to enter their username and initiates the password reset process
        * by verifying their identity through security questions. On successful verification,
        * the user is allowed to set a new password.
        * Errors during the process are handled and displayed to the user.
        *
        * @param userService The UserService instance used to reset the user's password.
        */
        static void ForgotPassword(UserService userService)
        {
            try
            {
                Console.Write("Enter your username: ");
                string username = Console.ReadLine();

                userService.ForgotPassword(username);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }


        /* Deletes the currently logged-in user's account.
        *
        * This method prompts the user for confirmation before deleting their account.
        * If confirmed, the account is deleted, and the current user is logged out.
        *
        * @param userService The UserService instance used to delete the user account.
        * @param currentUser The username of the currently logged-in user.
        */
        static void DeleteUser(UserService userService, string currentUser)
        {
            if (!CheckLoggedIn(currentUser)) return;

            Console.Write("Are you sure you want to delete your account? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                userService.DeleteUser(currentUser);
                Console.WriteLine($"User '{currentUser}' deleted successfully.");
                currentUser = null;
            }
        }

        /* Registers a library card for the currently logged-in user.
        *
        * This method checks if the user is logged in and prompts them to provide
        * the necessary details for library card registration.
        * A new library card is created and associated with the user.
        *
        * @param currentUser The username of the currently logged-in user.
        */
        static void RegisterLibraryCard(string? currentUser)
        {
            if (!CheckLoggedIn(currentUser)) return;

            LibraryCardService.CreateCard(currentUser);
            Console.WriteLine("Library card registered successfully.");
        }


        

        /* Allows a user to renew their library card early.
        *
        * This method checks if the user is logged in and if they have a library card.
        * If the user confirms the early renewal, the library card is renewed.
        * Otherwise, the process is canceled.
        *
        * @param currentUser The username of the currently logged-in user.
        */
        static void RenewCardEarly(string currentUser)
        {
            if (string.IsNullOrEmpty(currentUser))
            {
                Console.WriteLine("You must be logged in to renew your library card.");
                return;
            }

            // Retrieve the user's library card
            LibraryCard userCard = LibraryCardRepository.Instance.GetCard(currentUser);
            if (userCard == null)
            {
                Console.WriteLine($"Error: No library card found for user {currentUser}.");
                return;
            }

            Console.WriteLine("Are you sure you want to renew your library card early? (Y/N): ");
            string response = Console.ReadLine()?.Trim().ToUpper();

            if (response == "Y")
            {
                userCard.RenewCard();
            }
            else
            {
                Console.WriteLine("Early renewal canceled.");
            }
        }

        /* Displays the library card of the currently logged-in user.
        *
        * This method retrieves the user's library card and displays its details,
        * including borrowed and reserved books. If the user does not have a library card,
        * an appropriate message is displayed.
        *
        * @param currentUser The username of the currently logged-in user.
        */
        static void DisplayLibraryCard(string currentUser)
        {
            if (string.IsNullOrEmpty(currentUser))
            {
                Console.WriteLine("You must be logged in to view your library card.");
                return;
            }

            // Retrieve the user's library card
            LibraryCard userCard = LibraryCardRepository.Instance.GetCard(currentUser);
            if (userCard == null)
            {
                Console.WriteLine($"Error: No library card found for user {currentUser}.");
                return;
            }

            // Call the LibraryCard's DisplayLibraryCard method
            userCard.DisplayLibraryCard();
        }


        /* Displays the borrowed books of the currently logged-in user.
        *
        * This method retrieves and displays the list of borrowed books associated with the user's library card.
        * If the user is not logged in or does not have a library card, appropriate messages are displayed.
        *
        * @param currentUser The username of the currently logged-in user.
        */
        static void DisplayBorrowedBooks(string? currentUser)
        {
            if (!CheckLoggedIn(currentUser) || !CheckLibraryCard(currentUser)) return;

            var card = LibraryCardRepository.Instance.GetCard(currentUser);
            card.DisplayBooks();
        }

        /* Adds a new book to the library catalog.
        *
        * This method prompts the user for book details (ISBN, title, author, published year, and genre).
        * It then calls the BookService to add the book to the repository.
        * Only logged-in users can add books to the catalog.
        *
        * @param bookService The BookService instance used to manage books.
        * @param currentUser The username of the currently logged-in user.
        */
        static void AddBook(BookService bookService, string currentUser)
        {
            if (!CheckLoggedIn(currentUser)) return;

            Console.Write("Enter ISBN: ");
            var isbn = Console.ReadLine();
            Console.Write("Enter title: ");
            var title = Console.ReadLine();
            Console.Write("Enter author: ");
            var author = Console.ReadLine();
            Console.Write("Enter published year: ");
            if (!int.TryParse(Console.ReadLine(), out var publishedYear))
            {
                Console.WriteLine("Invalid year.");
                return;
            }
            Console.Write("Enter genre: ");
            var genre = Console.ReadLine();

            bookService.AddBook(isbn, title, author, publishedYear, genre, currentUser);
        }


        /* Allows a user to borrow a book from the library.
        *
        * This method displays all available books and prompts the user to enter the ISBN of the book they wish to borrow.
        * It verifies if the user is logged in and has a valid library card before proceeding.
        * The book is borrowed using the BookService.
        *
        * @param bookService The BookService instance used to manage books.
        * @param currentUser The username of the currently logged-in user.
        */
        static void BorrowBook(BookService bookService, string currentUser)
        {
            if (!CheckLoggedIn(currentUser) || !CheckLibraryCard(currentUser)) return;
            DisplayAvailableBooks(bookService, currentUser);
            Console.Write("Enter ISBN of the book to borrow: ");
            var isbn = Console.ReadLine();
            bookService.BorrowBook(isbn, currentUser);
        }

        /* Allows a user to return a borrowed book to the library.
        *
        * This method displays the books currently borrowed by the user and prompts them to enter the ISBN of the book they wish to return.
        * It verifies if the user is logged in and has a valid library card before proceeding.
        * The book is returned using the BookService.
        *
        * @param bookService The BookService instance used to manage books.
        * @param currentUser The username of the currently logged-in user.
        */
        static void ReturnBook(BookService bookService, string currentUser)
        {
            if (!CheckLoggedIn(currentUser) || !CheckLibraryCard(currentUser)) return;

            DisplayBorrowedBooks(currentUser);
            Console.Write("Enter ISBN of the book to return: ");
            var isbn = Console.ReadLine();
            bookService.ReturnBook(isbn, currentUser);
        }

        /* Allows a user to reserve a book in the library.
        *
        * This method displays all available books and prompts the user to enter the ISBN of the book they wish to reserve.
        * It verifies if the user is logged in before proceeding.
        * The book is reserved using the BookService.
        *
        * @param bookService The BookService instance used to manage books.
        * @param currentUser The username of the currently logged-in user.
        */
        static void ReserveBook(BookService bookService, string currentUser)
        {
            if (!CheckLoggedIn(currentUser)) return;

            DisplayAvailableBooks(bookService, currentUser);
            Console.Write("Enter ISBN of the book to reserve: ");
            var isbn = Console.ReadLine();
            bookService.ReserveBook(isbn, currentUser);
        }

        /* Allows a user to cancel a book reservation.
        *
        * This method prompts the user to enter the ISBN of the book whose reservation they wish to cancel.
        * It verifies if the user is logged in before proceeding.
        * The reservation is canceled using the BookService.
        *
        * @param bookService The BookService instance used to manage books.
        * @param currentUser The username of the currently logged-in user.
        */
        static void CancelReservation(BookService bookService, string currentUser)
        {
            if (!CheckLoggedIn(currentUser)) return;

            Console.Write("Enter ISBN of the book to cancel reservation: ");
            var isbn = Console.ReadLine();
            bookService.CancelReservation(isbn, currentUser);
        }


        /* Displays all books in the library catalog.
        *
        * This method retrieves and displays all books in the library, regardless of their availability status.
        * It verifies if the user is logged in before proceeding.
        *
        * @param bookService The BookService instance used to manage books.
        * @param currentUser The username of the currently logged-in user.
        */
        static void DisplayAllBooks(BookService bookService, string currentUser)
        {
            if (!CheckLoggedIn(currentUser)) return;

            bookService.DisplayAllBooks(currentUser);
        }

        /* Displays all available books in the library catalog.
        *
        * This method retrieves and displays books that are currently available (not borrowed or reserved).
        * It verifies if the user is logged in before proceeding.
        *
        * @param bookService The BookService instance used to manage books.
        * @param currentUser The username of the currently logged-in user.
        */
        static void DisplayAvailableBooks(BookService bookService, string currentUser)
        {
            if (!CheckLoggedIn(currentUser)) return;

            bookService.DisplayAvailableBooks(currentUser);
        }

        /* Searches for books in the library catalog by title.
        *
        * This method prompts the user to input a title or part of a title and searches for matching books in the catalog.
        * It verifies if the user is logged in and ensures that the input is valid before proceeding.
        *
        * @param bookService The BookService instance used to manage books.
        * @param currentUser The username of the currently logged-in user.
        */
        static void SearchBookByTitle(BookService bookService, string currentUser)
        {
            if (string.IsNullOrEmpty(currentUser))
            {
                Console.WriteLine("You must be logged in to search for books.");
                return;
            }

            Console.Write("Enter the title or part of the title to search for: ");
            string title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Invalid input. Title cannot be empty.");
                return;
            }

            // Call the BookService method to search by title
            bookService.SearchBookByTitle(title, currentUser);
        }


        /* Searches for books in the library catalog by author.
        *
        * This method prompts the user to input an author's name or part of it and searches for matching books in the catalog.
        * It verifies if the user is logged in and ensures that the input is valid before proceeding.
        *
        * @param bookService The BookService instance used to manage books.
        * @param currentUser The username of the currently logged-in user.
        */
        static void SearchBookByAuthor(BookService bookService, string currentUser)
        {
            if (string.IsNullOrEmpty(currentUser))
            {
                Console.WriteLine("You must be logged in to search for books.");
                return;
            }

            Console.Write("Enter the author's name or part of it to search for: ");
            string author = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(author))
            {
                Console.WriteLine("Invalid input. Author's name cannot be empty.");
                return;
            }

            // Call the BookService method to search by author
            bookService.SearchBookByAuthor(author, currentUser);
        }


        /* Updates the details of a book in the library catalog.
        *
        * This method prompts the user to input the ISBN of the book to update, along with any new details (title, author, 
        * published year, and genre). Fields can be left empty to remain unchanged.
        * It verifies if the user is logged in before proceeding.
        *
        * @param bookService The BookService instance used to manage books.
        * @param currentUser The username of the currently logged-in user.
        */
        static void UpdateBookDetails(BookService bookService, string currentUser)
        {
            if (string.IsNullOrEmpty(currentUser))
            {
                Console.WriteLine("You must be logged in to update a book.");
                return;
            }

            Console.Write("Enter the ISBN of the book to update: ");
            string isbn = Console.ReadLine();

            Console.Write("Enter the new title (leave empty to keep unchanged): ");
            string title = Console.ReadLine();

            Console.Write("Enter the new author (leave empty to keep unchanged): ");
            string author = Console.ReadLine();

            Console.Write("Enter the new published year (leave empty to keep unchanged): ");
            string publishedYearInput = Console.ReadLine();
            int publishedYear = 0;
            if (!string.IsNullOrWhiteSpace(publishedYearInput))
            {
                if (!int.TryParse(publishedYearInput, out publishedYear))
                {
                    Console.WriteLine("Invalid year format. Update canceled.");
                    return;
                }
            }

            Console.Write("Enter the new genre (leave empty to keep unchanged): ");
            string genre = Console.ReadLine();

            // Call the BookService method to update the book
            bookService.UpdateBook(isbn, title, author, publishedYear, genre, currentUser);
        }

        /* Deletes a book from the library catalog.
        *
        * This method prompts the user to input the ISBN of the book to delete and removes it from the catalog.
        * It verifies if the user is logged in and ensures that the ISBN is valid before proceeding.
        *
        * @param bookService The BookService instance used to manage books.
        * @param currentUser The username of the currently logged-in user.
        */
        static void DeleteBook(BookService bookService, string currentUser)
        {
            if (string.IsNullOrEmpty(currentUser))
            {
                Console.WriteLine("You must be logged in to delete a book.");
                return;
            }

            Console.Write("Enter the ISBN of the book to delete: ");
            string isbn = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(isbn))
            {
                Console.WriteLine("Invalid input. ISBN cannot be empty.");
                return;
            }

            // Call the BookService method to delete the book
            bookService.DeleteBook(isbn, currentUser);
        }
        
/* 
        ====================================================================================================
                                                HELPERS
        ====================================================================================================
*/
    
        /* Collects user input for username, password, and email.
        *
        * This helper method prompts the user to input their username, password, and email address. 
        * The collected information is returned as a tuple.
        *
        * @return A tuple containing the username, password, and email entered by the user.
        */
        static (string username, string password, string email) GetUserInput()
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();
            Console.Write("Enter email: ");
            var email = Console.ReadLine();
            return (username, password, email);
        }

        /* Checks if a user is currently logged in.
        *
        * This helper method verifies if the provided `currentUser` is not null, indicating that a user is logged in.
        * If no user is logged in, an appropriate message is displayed.
        *
        * @param currentUser The username of the currently logged-in user, or null if no user is logged in.
        * @return True if a user is logged in; otherwise, false.
        */
        static bool CheckLoggedIn(string? currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to perform this action.");
                return false;
            }
            return true;
        }

        /* Checks if the current user has a library card.
        *
        * This helper method verifies if the current user has a registered library card in the system.
        * If no library card exists for the user, an appropriate message is displayed.
        *
        * @param currentUser The username of the currently logged-in user.
        * @return True if the user has a library card; otherwise, false.
        */
        static bool CheckLibraryCard(string currentUser)
        {
            if (!LibraryCardRepository.Instance.ContainsUser(currentUser))
            {
                Console.WriteLine("You must have a library card to perform this action.");
                return false;
            }
            return true;
        }

    }
}
