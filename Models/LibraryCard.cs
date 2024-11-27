namespace Library_Management_System.Models
{
    /* 
     * Represents a library card for a user.
     *
     * This class manages the user's borrowed and reserved books, personal details, 
     * and the status of the library card (e.g., active, expired).
     */
    public class LibraryCard
    {
        // Static counter for unique User IDs
        private static int id = 100;

        // Maximum number of books that can be borrowed
        private const int MAXBOOKS = 5;

        // Lists to store borrowed and reserved books
        private List<Book> _borrowedBooks = new List<Book>();
        private List<Book> _reservedBooks = new List<Book>();

        // Properties for library card details
        public int UserId { get; } // Unique ID for the user
        public string FirstName { get; set; } // User's first name
        public string LastName { get; set; } // User's last name
        public Address Address { get; private set; } // User's address
        public DateTime IssueDate { get; private set; } // Date the card was issued
        public DateTime RenewalDate { get; private set; } // Date the card needs to be renewed
        public int BorrowedBookCount => _borrowedBooks.Count; // Number of borrowed books

        /* 
         * Initializes a new instance of the LibraryCard class.
         *
         * @param firstName The user's first name.
         * @param lastName The user's last name.
         * @param address The user's address.
         */
        public LibraryCard(string firstName, string lastName, Address address)
        {
            UserId = id++;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            IssueDate = DateTime.Now;
            RenewalDate = DateTime.Now.AddYears(1);
        }

        /* 
         * Updates the address associated with the library card.
         *
         * @param newAddress The new address to update.
         */
        public void UpdateAddress(Address newAddress)
        {
            Address = newAddress;
        }

        /* 
         * Checks if the library card is expired.
         *
         * @return True if the card is expired; otherwise, false.
         */
        public bool IsCardExpired()
        {
            return DateTime.Now > RenewalDate;
        }

        /* 
         * Renews the library card if it is expired.
         */
        public void RenewCard()
        {
            if (IsCardExpired())
            {
                IssueDate = DateTime.Now;
                RenewalDate = IssueDate.AddYears(1);
                Console.WriteLine("Library card renewed successfully.");
            }
            else
            {
                Console.WriteLine("Library card is still active. Renewal is not necessary.");
            }
        }

        /* 
         * Forces the renewal of the library card if it is expired.
         * Displays the status of the renewal process.
         */
        public void ForceRenewCard()
        {
            if (IsCardExpired())
            {
                Console.WriteLine("Your library card has expired and must be renewed.");
                Console.WriteLine("Renewing your library card...");
                IssueDate = DateTime.Now;
                RenewalDate = IssueDate.AddYears(1);
                Console.WriteLine($"Library card renewed successfully. New renewal date: {RenewalDate.ToShortDateString()}.");
            }
            else
            {
                Console.WriteLine("Your library card is still active. No renewal needed.");
            }
        }

        /* 
         * Borrows a book using the library card.
         *
         * @param book The book to borrow.
         * @throws InvalidOperationException If the borrowing limit is reached.
         */
        public void BorrowBook(Book book)
        {
            if (BorrowedBookCount >= MAXBOOKS)
            {
                throw new InvalidOperationException("Borrowing limit reached.");
            }

            _borrowedBooks.Add(book);
        }

        /* 
         * Reserves a book using the library card.
         *
         * @param book The book to reserve.
         * @throws InvalidOperationException If the book is already reserved by the user.
         */
        public void ReserveBook(Book book)
        {
            if (_reservedBooks.Contains(book))
            {
                throw new InvalidOperationException("This book is already reserved by you.");
            }

            _reservedBooks.Add(book);
        }

        /* 
         * Cancels a reservation for a book.
         *
         * @param book The book to cancel the reservation for.
         * @return True if the reservation was successfully canceled; otherwise, false.
         */
        public bool CancelReservation(Book book)
        {
            if (_reservedBooks.Contains(book))
            {
                _reservedBooks.Remove(book);
                return true;
            }
            return false;
        }

        /* 
         * Returns a borrowed book.
         *
         * @param book The book to return.
         * @return True if the book was successfully returned; otherwise, false.
         */
        public bool ReturnBook(Book book)
        {
            if (_borrowedBooks.Contains(book))
            {
                _borrowedBooks.Remove(book);
                return true;
            }
            return false;
        }

        /* 
         * Retrieves the list of borrowed books.
         *
         * @return A list of borrowed books.
         */
        public List<Book> GetBorrowedBooks()
        {
            return _borrowedBooks.ToList();
        }

        /* 
         * Retrieves the list of reserved books.
         *
         * @return A list of reserved books.
         */
        public List<Book> GetReservedBooks()
        {
            return _reservedBooks.ToList();
        }

        /* 
         * Displays all borrowed and reserved books associated with the library card.
         */
        public void DisplayBooks()
        {
            if (_borrowedBooks.Count == 0 && _reservedBooks.Count == 0)
            {
                Console.WriteLine("No borrowed or reserved books.");
                return;
            }

            if (_borrowedBooks.Count > 0)
            {
                Console.WriteLine("Borrowed Books:");
                foreach (var book in _borrowedBooks)
                {
                    Console.WriteLine($"ISBN: {book.ISBN}");
                    Console.WriteLine($"Title: {book.Title}");
                    Console.WriteLine($"Author: {book.Author}");
                    Console.WriteLine($"Published Year: {book.PublishedYear}");
                    Console.WriteLine($"Genre: {book.Genre}");
                    Console.WriteLine();
                }
            }

            if (_reservedBooks.Count > 0)
            {
                Console.WriteLine("Reserved Books:");
                foreach (var book in _reservedBooks)
                {
                    Console.WriteLine($"ISBN: {book.ISBN}");
                    Console.WriteLine($"Title: {book.Title}");
                    Console.WriteLine($"Author: {book.Author}");
                    Console.WriteLine($"Published Year: {book.PublishedYear}");
                    Console.WriteLine($"Genre: {book.Genre}");
                    Console.WriteLine();
                }
            }
        }

        /* 
         * Displays the details of the library card.
         */
        public void DisplayLibraryCard()
        {
            Console.WriteLine("===== Library Card Details =====");
            Console.WriteLine($"User ID: {UserId}");
            Console.WriteLine($"Name: {FirstName} {LastName}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Issue Date: {IssueDate.ToShortDateString()}");
            Console.WriteLine($"Renewal Date: {RenewalDate.ToShortDateString()}");
            Console.WriteLine($"Card Status: {(IsCardExpired() ? "Expired" : "Active")}");

            // Display Borrowed Books
            if (_borrowedBooks.Count > 0)
            {
                Console.WriteLine("\nBorrowed Books:");
                foreach (var book in _borrowedBooks)
                {
                    Console.WriteLine($"- ISBN: {book.ISBN}, Title: {book.Title}, Author: {book.Author}");
                }
            }
            else
            {
                Console.WriteLine("\nNo Borrowed Books.");
            }

            // Display Reserved Books
            if (_reservedBooks.Count > 0)
            {
                Console.WriteLine("\nReserved Books:");
                foreach (var book in _reservedBooks)
                {
                    Console.WriteLine($"- ISBN: {book.ISBN}, Title: {book.Title}, Author: {book.Author}");
                }
            }
            else
            {
                Console.WriteLine("\nNo Reserved Books.");
            }

            Console.WriteLine("===============================");
        }
    }
}
