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
            // We can use logger if we need to instead of printing to the console.
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

            // Retrieve the user's library card
            LibraryCard userCard = LibraryCardRepository.Instance.GetCard(username);
            if (userCard == null)
            {
                Console.WriteLine($"Error: No library card found for user {username}.");
                return;
            }

            // Validate borrowing conditions and borrow the book
            try
            {
                if (userCard.IsCardExpired())
                {
                    Console.WriteLine("Error: Your library card is expired. Please renew it to borrow books.");
                    userCard.ForceRenewCard();
                    return;
                }

                if (userCard.BorrowedBookCount >= 5)
                {
                    Console.WriteLine("Error: Borrowing limit reached. Please return a book to borrow a new one.");
                    return;
                }

                if (!book.IsAvailable)
                {
                    Console.WriteLine($"Error: Book '{book.Title}' is currently not available for borrowing.");
                    return;
                }

                // Borrow the book
                userCard.BorrowBook(book);
                book.Borrow(username);
                Console.WriteLine($"Success: Book '{book.Title}' borrowed successfully by {username}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Unable to borrow book. {ex.Message}");
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

            // Retrieve the user's library card
            LibraryCard userCard = LibraryCardRepository.Instance.GetCard(username);
            if (userCard == null)
            {
                Console.WriteLine($"Error: No library card found for user {username}.");
                return;
            }

            // Check if the book is currently borrowed by this user
            if (!book.IsBorrowed || book.BorrowedByUser != username)
            {
                Console.WriteLine($"Error: The book '{book.Title}' is not borrowed by {username}.");
                return;
            }

            // Return the book
            try
            {
                if (userCard.ReturnBook(book))
                {
                    book.Return();
                    Console.WriteLine($"Success: Book '{book.Title}' returned successfully.");
                }
                else
                {
                    Console.WriteLine($"Error: Unable to return book '{book.Title}'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Unable to return book. {ex.Message}");
            }
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

            // Retrieve the user's library card
            LibraryCard userCard = LibraryCardRepository.Instance.GetCard(username);
            if (userCard == null)
            {
                Console.WriteLine($"Error: No library card found for user {username}.");
                return;
            }

            // Attempt to reserve the book
            try
            {
                if (!book.IsAvailable)
                {
                    Console.WriteLine($"Error: Book '{book.Title}' is not available for reservation.");
                    return;
                }

                userCard.ReserveBook(book);
                book.Reserve(username);
                Console.WriteLine($"Book '{book.Title}' reserved successfully by {username}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Unable to reserve book. {ex.Message}");
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

            // Retrieve the user's library card
            LibraryCard userCard = LibraryCardRepository.Instance.GetCard(username);
            if (userCard == null)
            {
                Console.WriteLine($"Error: No library card found for user {username}.");
                return;
            }

            // Attempt to cancel the reservation
            try
            {
                if (!userCard.GetReservedBooks().Contains(book))
                {
                    Console.WriteLine($"Error: Book '{book.Title}' is not reserved by {username}.");
                    return;
                }

                // Remove reservation from the user's library card
                if (userCard.CancelReservation(book))
                {
                    // Update the book's reservation status
                    book.CancelReservation(username);
                    Console.WriteLine($"Success: Reservation for book '{book.Title}' by {username} canceled successfully.");
                }
                else
                {
                    Console.WriteLine($"Error: Unable to cancel reservation for book '{book.Title}'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Unable to cancel reservation. {ex.Message}");
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
            // Ensure required users are set up
            SetupUsers();

            // Fetch the admin user to perform book addition
            var adminUser = userRepository.GetUserByUsername("admin_user");
            if (adminUser == null || !adminUser.IsLoggedIn)
            {
                Console.WriteLine("Admin user setup failed or is not logged in. Cannot pre-add books.");
                return;
            }

            // Predefined list of books with fake ISBNs
            var books = new List<(string Isbn, string Title, string Author, int PublishedYear, string Genre)>
            {
                ("111-2223334450", "Pride and Prejudice", "Jane Austen", 1813, "Fiction"),
                ("111-2223334451", "To Kill a Mockingbird", "Harper Lee", 1960, "Fiction"),
                ("111-2223334452", "The Great Gatsby", "F. Scott Fitzgerald", 1925, "Fiction"),
                ("111-2223334453", "1984", "George Orwell", 1949, "Fiction"),
                ("111-2223334454", "The Catcher in the Rye", "J.D. Salinger", 1951, "Fiction"),
                ("111-2223334455", "Moby Dick", "Herman Melville", 1851, "Fiction"),
                ("111-2223334456", "War and Peace", "Leo Tolstoy", 1869, "Fiction"),
                ("111-2223334457", "The Odyssey", "Homer", -800, "Fiction"),
                ("111-2223334458", "The Iliad", "Homer", -750, "Fiction"),
                ("111-2223334459", "Crime and Punishment", "Fyodor Dostoevsky", 1866, "Fiction"),
                ("111-2223334460", "Frankenstein", "Mary Shelley", 1818, "Fiction"),
                ("111-2223334461", "Dracula", "Bram Stoker", 1897, "Fiction"),
                ("111-2223334462", "Jane Eyre", "Charlotte Bronte", 1847, "Fiction"),
                ("111-2223334463", "The Lord of the Rings", "J.R.R. Tolkien", 1954, "Fiction"),
                ("111-2223334464", "Animal Farm", "George Orwell", 1945, "Fiction"),
                ("111-2223334465", "Brave New World", "Aldous Huxley", 1932, "Fiction"),
                ("111-2223334466", "Wuthering Heights", "Emily Bronte", 1847, "Fiction"),
                ("111-2223334467", "Don Quixote", "Miguel de Cervantes", 1605, "Fiction"),
                ("111-2223334468", "The Divine Comedy", "Dante Alighieri", 1320, "Fiction"),
                ("111-2223334469", "Hamlet", "William Shakespeare", 1600, "Fiction")
            };

            // Add books
            foreach (var book in books)
            {
                try
                {
                    AddBook(book.Isbn, book.Title, book.Author, book.PublishedYear, book.Genre, adminUser.Username);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to pre-add book '{book.Title}': {ex.Message}");
                }
            }
            adminUser.IsLoggedIn = false;
            Console.WriteLine("Pre-added books successfully.");
        }

        private void SetupUsers()
        {
            const string adminUsername = "admin_user";
            const string adminPassword = "Admin@123";
            const string adminEmail = "admin@admin.com";

            const string userUsername = "regular_user";
            const string userPassword = "User@123";
            const string userEmail = "user@user.com";

            var adminUser = userRepository.GetUserByUsername(adminUsername);
            var regularUser = userRepository.GetUserByUsername(userUsername);

            if (adminUser == null)
            {
                adminUser = new User(adminUsername, adminEmail, Role.ADMIN);
                regularUser = new User(userUsername, userEmail, Role.USER);
                try
                {
                    adminUser.Password.SetPassword(adminPassword);
                    regularUser.Password.SetPassword(userPassword);
                    userRepository.AddUser(adminUser);
                    userRepository.AddUser(regularUser);
                    Console.WriteLine("Users created successfully.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Failed to set user passwords: {ex.Message}");
                    return;
                }
            }

            adminUser.IsLoggedIn = true;
            regularUser.IsLoggedIn = false;
        }
    }
}
