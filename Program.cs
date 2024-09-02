
// using Library_Management_System.Services;
// using Library_Management_System.Models;

// namespace Library_Management_System
// {
//     class Program
//     {
//         static void Main(string[] args)
//         {
//             Console.WriteLine("Starting the application...");

//             // User service interactions
//             var userService = new UserService();

//             // Register a new user (default role: USER)
//             userService.Register("john_doe", "password123", "john@example.com");
//             // Expected Outcome: User 'john_doe' should be successfully registered.

//             // Register an admin user
//             userService.Register("admin_user", "adminpass", "admin@example.com", Role.ADMIN);
//             // Expected Outcome: Admin 'admin_user' should be successfully registered.

//             // Log in the regular user
//             userService.Login("john_doe", "password123");
//             // Expected Outcome: User 'john_doe' should log in successfully.

//             // Log out the regular user
//             userService.Logout("john_doe");
//             // Expected Outcome: User 'john_doe' should log out successfully.

//             // Log in the regular user again
//             userService.Login("john_doe", "password123");
//             // Expected Outcome: User 'john_doe' should log in successfully.

//             // Log in the admin user
//             userService.Login("admin_user", "adminpass");
//             // Expected Outcome: Admin 'admin_user' should log in successfully.

//             // Book service interactions
//             var bookService = new BookService();

//             // Attempt to add a book with the regular user (should fail)
//             bookService.AddBook("123-4567891234", "Effective C#", "Bill Wagner", 2017, "Programming", "john_doe");

//             // Add 10 books with increasing volumes and publication years using a loop
//             for (int i = 1; i <= 5; i++)
//             {
//                 string isbn = $"987-6543210{i:D3}"; // Generates ISBNs like 987-654321001, 987-654321002, ...
//                 string title = $"Effective C# Vol. {i}";
//                 int publishedYear = 2020 + i;
                
//                 bookService.AddBook(isbn, title, "Bill Wagner", publishedYear, "Programming", "admin_user");
//             }

//             // Display all books with the admin user
//             bookService.DisplayAllBooks("admin_user");

//             // Attempt to delete a book with the regular user (should fail)
//             bookService.DeleteBook("9987-6543210001", "john_doe");

//             // Delete a book with the admin user (should succeed)
//             bookService.DeleteBook("987-6543210001", "admin_user");

//             // Attempt to update a book's details with the regular user (should fail)
//             bookService.UpdateBook("987-6543210002", "Advanced C#", "Bill Wagner", 2025, "Programming", "john_doe");

//             // Update a book's details with the admin user (should succeed)
//             bookService.UpdateBook("987-6543210002", "Advanced C# Vol. 2", "Bill Wagner", 2025, "Programming", "admin_user");

//             // Display all books with the admin user after modifications
//             bookService.DisplayAllBooks("admin_user");

//             // Borrow a book with the regular user
//             bookService.BorrowBook("987-6543210003", "john_doe");

//             // Display available books (should not include the borrowed book)
//             bookService.DisplayAvailableBooks("john_doe");

//             // Return the borrowed book
//             bookService.ReturnBook("987-6543210003", "john_doe");

//             // Display available books again (should include the returned book)
//             bookService.DisplayAvailableBooks("john_doe");

//             // Delete the regular user
//             userService.DeleteUser("john_doe");
//             // Expected Outcome: User 'john_doe' should be deleted successfully.

//             // Log out the admin user
//             userService.Logout("admin_user");
//             // Expected Outcome: Admin 'admin_user' should log out successfully.

//             // Allow time for logging to complete
//             Thread.Sleep(500); // A small delay before the program ends

//             Console.WriteLine("Program executed successfully.");
//         }
//     }
// }

using Library_Management_System.Services;
using Library_Management_System.Models;
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
                Console.WriteLine("\nPlease select an option:");
                Console.WriteLine("1. Register User");
                Console.WriteLine("2. Register Admin");
                Console.WriteLine("3. Log In");
                Console.WriteLine("4. Log Out");
                Console.WriteLine("5. Add Book");
                Console.WriteLine("6. Borrow Book");
                Console.WriteLine("7. Return Book");
                Console.WriteLine("8. Reserve Book");
                Console.WriteLine("9. Cancel Reservation");
                Console.WriteLine("10. Display All Books");
                Console.WriteLine("11. Display Available Books");
                Console.WriteLine("12. Delete User");
                Console.WriteLine("13. Exit");

                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
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
                        AddBook(bookService, currentUser);
                        break;
                    case "6":
                        BorrowBook(bookService, currentUser);
                        break;
                    case "7":
                        ReturnBook(bookService, currentUser);
                        break;
                    case "8":
                        ReserveBook(bookService, currentUser);
                        break;
                    case "9":
                        CancelReservation(bookService, currentUser);
                        break;
                    case "10":
                        DisplayAllBooks(bookService, currentUser);
                        break;
                    case "11":
                        DisplayAvailableBooks(bookService, currentUser);
                        break;
                    case "12":
                        DeleteUser(userService, currentUser);
                        break;
                    case "13":
                        Console.WriteLine("Exiting the program...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void RegisterUser(UserService userService)
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();
            Console.Write("Enter email: ");
            var email = Console.ReadLine();

            if (userService.Register(username, password, email))
            {
                Console.WriteLine($"User '{username}' registered successfully.");
            }
            else
            {
                Console.WriteLine($"Registration failed: Username '{username}' already exists.");
            }
        }

        static void RegisterAdmin(UserService userService)
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();
            Console.Write("Enter email: ");
            var email = Console.ReadLine();

            if (userService.Register(username, password, email, Role.ADMIN))
            {
                Console.WriteLine($"Admin '{username}' registered successfully.");
            }
            else
            {
                Console.WriteLine($"Registration failed: Username '{username}' already exists.");
            }
        }

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

        static void AddBook(BookService bookService, string currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to add a book.");
                return;
            }

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

        static void BorrowBook(BookService bookService, string currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to borrow a book.");
                return;
            }

            Console.Write("Enter ISBN of the book to borrow: ");
            var isbn = Console.ReadLine();
            bookService.BorrowBook(isbn, currentUser);
        }

        static void ReturnBook(BookService bookService, string currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to return a book.");
                return;
            }

            Console.Write("Enter ISBN of the book to return: ");
            var isbn = Console.ReadLine();
            bookService.ReturnBook(isbn, currentUser);
        }

        static void ReserveBook(BookService bookService, string currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to reserve a book.");
                return;
            }

            Console.Write("Enter ISBN of the book to reserve: ");
            var isbn = Console.ReadLine();
            bookService.ReserveBook(isbn, currentUser);
        }

        static void CancelReservation(BookService bookService, string currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to cancel a reservation.");
                return;
            }

            Console.Write("Enter ISBN of the book to cancel the reservation: ");
            var isbn = Console.ReadLine();
            bookService.CancelReservation(isbn, currentUser);
        }

        static void DisplayAllBooks(BookService bookService, string currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to view all books.");
                return;
            }

            bookService.DisplayAllBooks(currentUser);
        }

        static void DisplayAvailableBooks(BookService bookService, string currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to view available books.");
                return;
            }

            bookService.DisplayAvailableBooks(currentUser);
        }

        static void DeleteUser(UserService userService, string currentUser)
        {
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to delete your account.");
                return;
            }

            Console.Write("Are you sure you want to delete your account? (y/n): ");
            var confirm = Console.ReadLine();
            if (confirm?.ToLower() == "y")
            {
                userService.DeleteUser(currentUser);
                Console.WriteLine($"User '{currentUser}' deleted successfully.");
                currentUser = null;
            }
        }
    }
}
