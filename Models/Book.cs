namespace Library_Management_System.Models
{
    /* 
     * Represents a book in the library.
     *
     * This class encapsulates all the properties and behaviors of a book, 
     * including its borrowing and reservation status, and provides methods 
     * to manage the book's state.
     */
    public class Book
    {
        // Encapsulated properties with getters and private setters to protect data integrity.
        public string ISBN { get; private set; } // Unique identifier for the book
        public string Title { get; private set; } // Title of the book
        public string Author { get; private set; } // Author of the book
        public int PublishedYear { get; private set; } // Year the book was published
        public string Genre { get; private set; } // Genre of the book
        public bool IsAvailable => !IsBorrowed && string.IsNullOrEmpty(ReservedByUser); // True if not borrowed or reserved
        public bool IsBorrowed { get; private set; } // Indicates if the book is currently borrowed
        public string? BorrowedByUser { get; private set; } // Username of the user who borrowed the book
        public string? ReservedByUser { get; private set; } // Username of the user who reserved the book

        /* 
         * Initializes a new instance of the Book class with the specified details.
         *
         * @param isbn The unique ISBN of the book.
         * @param title The title of the book.
         * @param author The author of the book.
         * @param publishedYear The year the book was published.
         * @param genre The genre of the book.
         */
        public Book(string isbn, string title, string author, int publishedYear, string genre)
        {
            ISBN = isbn;
            Title = title;
            Author = author;
            PublishedYear = publishedYear;
            Genre = genre;
            IsBorrowed = false;
            BorrowedByUser = null;
            ReservedByUser = null;
        }

        /* 
         * Updates the book's information.
         *
         * This method allows updating the title, author, published year, and genre of the book.
         *
         * @param title The new title of the book.
         * @param author The new author of the book.
         * @param publishedYear The new published year of the book.
         * @param genre The new genre of the book.
         */
        public void UpdateBookInfo(string title, string author, int publishedYear, string genre)
        {
            Title = title;
            Author = author;
            PublishedYear = publishedYear;
            Genre = genre;
        }

        /* 
         * Marks the book as borrowed by a user.
         *
         * @param username The username of the user borrowing the book.
         * @throws InvalidOperationException If the book is already borrowed or reserved by another user.
         */
        public void Borrow(string username)
        {
            if (IsBorrowed)
                throw new InvalidOperationException("Book is already borrowed.");

            if (!string.IsNullOrEmpty(ReservedByUser) && ReservedByUser != username)
                throw new InvalidOperationException("Book is reserved by another user.");

            IsBorrowed = true;
            BorrowedByUser = username;
            ReservedByUser = null; // Clear reservation if the user borrows the book
        }

        /* 
         * Marks the book as returned.
         */
        public void Return()
        {
            IsBorrowed = false;
            BorrowedByUser = null;
        }

        /* 
         * Reserves the book for a user.
         *
         * @param username The username of the user reserving the book.
         * @throws InvalidOperationException If the book is already borrowed or reserved.
         */
        public void Reserve(string username)
        {
            if (IsBorrowed)
                throw new InvalidOperationException("Book is already borrowed.");

            if (!string.IsNullOrEmpty(ReservedByUser))
                throw new InvalidOperationException("Book is already reserved.");

            ReservedByUser = username;
        }

        /* 
         * Cancels a reservation for the book.
         *
         * @param username The username of the user canceling the reservation.
         * @throws InvalidOperationException If the reservation does not belong to the specified user.
         */
        public void CancelReservation(string username)
        {
            if (ReservedByUser == username)
            {
                ReservedByUser = null;
            }
            else
            {
                throw new InvalidOperationException("Reservation can only be canceled by the user who reserved the book.");
            }
        }

        /* 
         * Returns a string representation of the book.
         *
         * This method provides a formatted string with all relevant details about the book.
         *
         * @return A string containing the book's details.
         */
        public override string ToString()
        {
            return $"ISBN: {ISBN}\n" +
                   $"Title: {Title}\n" +
                   $"Author: {Author}\n" +
                   $"Published Year: {PublishedYear}\n" +
                   $"Genre: {Genre}\n" +
                   $"Available: {IsAvailable}\n";
        }
    }
}
