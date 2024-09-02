using Library_Management_System.Models;
using Library_Management_System.Repositories;
using Microsoft.Extensions.Logging;
using Library_Management_System.Logging;


namespace Library_Management_System.Services
{
    public class BookService
    {
        private readonly BookRepository bookRepository = BookRepository.Instance;
        private readonly UserRepository userRepository = UserRepository.Instance;
        private readonly ILogger<BookService> _logger;


        public BookService()
        {
            // We can use logger if we need o instead of printing to the console.
            //TODO: Discuss this in the sync up
            _logger = LoggingConfig.CreateLogger<BookService>();
        }


        /* Adds a new book to the repository if the user is verified.
        *
        * This method creates a new Book object and adds it to the repository.
        * It first checks if the user is verified (exists and is logged in).
        * Then, it checks if a book with the same ISBN already exists before adding it.
        *
        * @param isbn The ISBN of the book.
        * @param title The title of the book.
        * @param author The author of the book.
        * @param publishedYear The year the book was published.
        * @param genre The genre of the book.
        * @param username The username of the user attempting to add the book.
        */
        public void AddBook(string isbn, string title, string author, int publishedYear, string genre, string username)
        {
            // Verify if the user exists and is logged in
            if (!VerifyUser(username))
            {
                return; // Exit if the user is not verified
            }

            if (!UserIsStaff(username)){
                return; // Exit if the user is not
            }

            // Check if a book with the same ISBN already exists
            var existingBook = bookRepository.GetBookByISBN(isbn);
            if (existingBook != null)
            {
                Console.WriteLine($"Error: A book with ISBN {isbn} already exists.");
                return; // Exit if the book already exists
            }

            // Create a new Book object and add it to the repository
            var book = new Book(isbn, title, author, publishedYear, genre);
            bookRepository.AddBook(book);
            Console.WriteLine("Book added successfully.");
        }
    

        /* Searches for books by title if the user is verified.
        *
        * This method retrieves books from the repository that match the specified title.
        * It first checks if the user is verified (exists and is logged in) before proceeding with the search.
        *
        * @param title The title or part of the title to search for.
        * @param username The username of the user attempting to search for books.
        */
        public void SearchBookByTitle(string title, string username)
        {
            // Verify if the user exists and is logged in
            if (!VerifyUser(username))
            {
                return; // Exit if the user is not verified
            }
            var books = bookRepository.GetBooksByTitle(title);
            if (books.Count > 0)
            {
               Console.WriteLine($"Books matching title '{title}':");
                foreach (var book in books)
                {
                    Console.WriteLine(book.ToString());
                }
            }
            else
            {
                Console.WriteLine("No books found with that title.");
            }
        }

        /* Searches for books by author.
        *
        * This method retrieves books from the repository that match the specified author.
        *
        * @param author The author or part of the author's name to search for.
        */
        public void SearchBookByAuthor(string author, string username)
        {

            // Verify if the user exists and is logged in
            if (!VerifyUser(username))
            {
                return; // Exit if the user is not verified
            }

            var books = bookRepository.GetBooksByAuthor(author);
            if (books.Count > 0)
            {
                Console.WriteLine($"Books by author '{author}':");
                foreach (var book in books)
                {
                    Console.WriteLine(book.ToString());
                }
            }
            else
            {
                Console.WriteLine("No books found by that author.");
            }
        }

        /* Displays all books in the catalog.
        *
        * This method retrieves all books from the repository and displays them.
        */
        public void DisplayAllBooks(string username)
        {
            // Verify if the user exists and is logged in
            if (!VerifyUser(username))
            {
                return; // Exit if the user is not verified
            }

            if (!UserIsStaff(username))
            {
                return; // Exit if the user is not
            }

            var books = bookRepository.GetAllBooks();
            if (books.Count > 0)
            {
                Console.WriteLine("All Books in the Catalog:");
                Console.WriteLine("There are " + books.Count + " books in the catalog.");
                foreach (var book in books)
                {
                    Console.WriteLine(book.ToString());
                }
            }
            else
            {
               Console.WriteLine("No books available in the catalog.");
            }
        }

        /* Deletes a book by its ISBN.
        *
        * This method deletes a book from the repository based on its ISBN.
        * It checks if the book exists before attempting to delete it.
        *
        * @param isbn The ISBN of the book to delete.
        */
        public void DeleteBook(string isbn, string username)
        {
            // Verify if the user exists and is logged in
            if (!VerifyUser(username))
            {
                return; // Exit if the user is not verified
            }

            if (!UserIsStaff(username))
            {
                return; // Exit if the user is not
            }

            var book = bookRepository.GetBookByISBN(isbn);
            if (book == null)
            {
                Console.WriteLine($"Error: No book found with ISBN {isbn}.");
                return;
            }

            bookRepository.DeleteBook(isbn);
            Console.WriteLine("Book deleted successfully.");
        }

        /* Updates a book's information.
        *
        * This method updates the details of an existing book in the repository.
        * It checks if the book exists before attempting to update it.
        *
        * @param isbn The ISBN of the book to update.
        * @param title The new title of the book.
        * @param author The new author of the book.
        * @param publishedYear The new published year of the book.
        * @param genre The new genre of the book.
        */
        public void UpdateBook(string isbn, string title, string author, int publishedYear, string genre, string username)
        {
            // Verify if the user exists and is logged in
            if (!VerifyUser(username))
            {
                return; // Exit if the user is not verified
            }

            if (!UserIsStaff(username)){

                return; // Exit if the user is not
            }


            var book = bookRepository.GetBookByISBN(isbn);
            if (book == null)
            {
                Console.WriteLine($"Error: No book found with ISBN {isbn}.");
                return;
            }

            bookRepository.UpdateBook(isbn, title, author, publishedYear, genre);
            Console.WriteLine("Book updated successfully.");
        }

        

        /* Borrows a book from the library if the user is verified.
        *
        * This method allows a user to borrow a book from the repository by its ISBN.
        * It first verifies that the user exists and is logged in, then attempts to borrow the book.
        * If the book is already borrowed or reserved by another user, an appropriate error is logged.
        *
        * @param isbn The ISBN of the book to borrow.
        * @param username The username of the user attempting to borrow the book.
        */
        public void BorrowBook(string isbn, string username)
        {
            // Verify if the user exists and is logged in
            if (!VerifyUser(username))
            {
                return; // Exit if the user is not verified
            }

            // Retrieve the book by ISBN
            var book = bookRepository.GetBookByISBN(isbn);
            if (book == null)
            {
                Console.WriteLine($"Error: No book found with ISBN {isbn}.");
                return; // Exit if the book does not exist
            }

            // Attempt to borrow the book within a try-catch block
            try
            {
                book.Borrow(username);
                Console.WriteLine($"Book '{book.Title}' borrowed successfully by {username}.");
            }
            catch (InvalidOperationException ex)
            {
                // Handle any exceptions thrown by the Borrow method
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        /* Returns a borrowed book to the library if the user is verified.
        *
        * This method allows a user to return a borrowed book by its ISBN.
        * It first verifies that the user exists and is logged in, then checks if the book is currently borrowed.
        * If the book is not found or is not borrowed, an appropriate error is logged.
        *
        * @param isbn The ISBN of the book to return.
        * @param username The username of the user attempting to return the book.
        */
        public void ReturnBook(string isbn, string username)
        {
            // Verify if the user exists and is logged in
            if (!VerifyUser(username))
            {
                return; // Exit if the user is not verified
            }

            // Retrieve the book by ISBN
            var book = bookRepository.GetBookByISBN(isbn);
            if (book == null)
            {
                Console.WriteLine($"Error: No book found with ISBN {isbn}.");
                return; // Exit if the book does not exist
            }

            // Check if the book is currently borrowed
            if (!book.IsBorrowed)
            {
                Console.WriteLine($"Error: The book '{book.Title}' is not currently borrowed.");
                return; // Exit if the book is not borrowed
            }

            // Mark the book as returned
            book.Return();
            Console.WriteLine($"Book '{book.Title}' returned successfully.");
        }


        /* Reserves a book if the user is verified.
        *
        * This method allows a user to reserve a book by its ISBN.
        * It first verifies that the user exists and is logged in, then attempts to reserve the book.
        * If the book is already reserved or borrowed, an appropriate error is logged.
        *
        * @param isbn The ISBN of the book to reserve.
        * @param username The username of the user attempting to reserve the book.
        */
        public void ReserveBook(string isbn, string username)
        {
            // Verify if the user exists and is logged in
            if (!VerifyUser(username))
            {
                return; // Exit if the user is not verified
            }

            // Retrieve the book by ISBN
            var book = bookRepository.GetBookByISBN(isbn);
            if (book == null)
            {
                Console.WriteLine($"Error: No book found with ISBN {isbn}.");
                return; // Exit if the book does not exist
            }

            // Attempt to reserve the book within a try-catch block
            try
            {
                book.Reserve(username);
                Console.WriteLine($"Book '{book.Title}' reserved successfully by {username}.");
            }
            catch (InvalidOperationException ex)
            {
                // Handle any exceptions thrown by the Reserve method
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /* Displays all available books in the catalog if the user is verified.
        *
        * This method retrieves and displays books that are available in the library (not borrowed or reserved).
        * It first verifies that the user exists and is logged in before proceeding.
        *
        * @param username The username of the user attempting to view available books.
        */
        public void DisplayAvailableBooks(string username)
        {
            // Verify if the user exists and is logged in
            if (!VerifyUser(username))
            {
                return; // Exit if the user is not verified
            }

            // Retrieve all available books (books that are not borrowed or reserved)
            var availableBooks = bookRepository.GetAllBooks()
                                        .Where(b => b.IsAvailable)
                                        .ToList();

            // Check if there are any available books
            if (availableBooks.Count > 0)
            {
                Console.WriteLine("There are " + availableBooks.Count + " available books in the catalog:");
                foreach (var book in availableBooks)
                {
                    Console.WriteLine(book.ToString());
                }
            }
            else
            {
                Console.WriteLine("No available books in the catalog.");
            }
        }

        /* Cancels a book reservation if the user is verified.
        *
        * This method allows a user to cancel their reservation of a book by its ISBN.
        * It first verifies that the user exists and is logged in, then attempts to cancel the reservation.
        * If the reservation does not exist or belongs to another user, an appropriate error is logged.
        *
        * @param isbn The ISBN of the book to cancel the reservation.
        * @param username The username of the user attempting to cancel the reservation.
        */
        public void CancelReservation(string isbn, string username)
        {
            // Verify if the user exists and is logged in
            if (!VerifyUser(username))
            {
                return; // Exit if the user is not verified
            }

            // Retrieve the book by ISBN
            var book = bookRepository.GetBookByISBN(isbn);
            if (book == null)
            {
                Console.WriteLine($"Error: No book found with ISBN {isbn}.");
                return; // Exit if the book does not exist
            }

            // Attempt to cancel the reservation within a try-catch block
            try
            {
                book.CancelReservation(username);
                Console.WriteLine($"Reservation for book '{book.Title}' by {username} canceled successfully.");
            }
            catch (InvalidOperationException ex)
            {
                // Handle any exceptions thrown by the CancelReservation method
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
/* 
        ====================================================================================================
                                                HELPERS
        ====================================================================================================
*/


        /* Verifies if a user exists and is logged in.
        *
        * This method checks if the user exists and is currently logged in.
        * If the user does not exist or is not logged in, an appropriate error is logged and the method returns false.
        *
        * @param username The username of the user to verify.
        * @return True if the user exists and is logged in; otherwise, false.
        */
        private bool VerifyUser(string username)
        {
            if (!userRepository.IsUserLoggedIn(username))
            {
                Console.WriteLine($"Error: User '{username}' does not exist or is not logged in.");
                return false;
            }
            return true;
        }

        /* Checks if the user is a staff member.
        *
        * This method verifies if the user has staff privileges.
        * It checks the user's role to determine if they are a staff member.
        *
        * @param username The username of the user to check.
        * @return True if the user is a staff member; otherwise, false.
        */

        private bool UserIsStaff(string username)
        {
            var user = userRepository.GetUserByUsername(username);

             // Check if the user exists
            if (user == null)
            {
                Console.WriteLine($"Error: User '{username}' does not exist.");
                return false; // Exit if the user does not exist
            }

            // Check if the user has the STAFF or ADMIN role
            if (user.Role != Role.STAFF && user.Role != Role.ADMIN)
            {
                Console.WriteLine($"Error: User '{username}' does not have permission for this action.");
                return false; // Exit if the user does not have the required role
            }

            return true;
        }

        /* Pre-adds 20 books to the repository before the application starts.
         *
         * This method creates 20 different Book objects with varying ISBNs, titles, authors, and published years.
         * It adds these books to the repository so that they are available when the application starts.
         */
        public void PreAddBooks()
        {
            //Create an admin user and log them in
            //TODO: Add actual books from an API or mock book objects
            var adminUser = new User("admin_user", "admin123", "admin@admin.com", Role.ADMIN);
            userRepository.AddUser(adminUser);
            adminUser.IsLoggedIn = true;


            for (int i = 1; i <= 20; i++)
            {
                string isbn = $"111-22233344{i:D2}";
                string title = $"Book Title {i}";
                string author = $"Author {i}";
                int publishedYear = 2000 + i;
                string genre = (i % 2 == 0) ? "Fiction" : "Non-Fiction";

                // Assuming the 'admin_user' is used to pre-add the books
                AddBook(isbn, title, author, publishedYear, genre, adminUser.Username);
            }

            Console.WriteLine("Pre-added 20 books to the repository.");
        }
    }
}
