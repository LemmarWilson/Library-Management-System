namespace Library_Management_System.Models
{
    public class Book
    {
        // Encapsulated properties with getters and private setters to protect the integrity of the data.
        public string ISBN { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public int PublishedYear { get; private set; }
        public string Genre { get; private set; }
        public bool IsAvailable => !IsBorrowed && string.IsNullOrEmpty(ReservedByUser); // Available if not borrowed or reserved
        public bool IsBorrowed { get; private set; }
        public string? BorrowedByUser { get; private set; }
        public string? ReservedByUser { get; private set; }

        // Constructor to initialize the Book object
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

        // Method to update book information
        public void UpdateBookInfo(string title, string author, int publishedYear, string genre)
        {
            Title = title;
            Author = author;
            PublishedYear = publishedYear;
            Genre = genre;
        }

        // This method handles the logic of marking the book as borrowed.
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

        // This method handles the logic of marking the book as returned.
        public void Return()
        {
            IsBorrowed = false;
            BorrowedByUser = null;
        }

        public void Reserve(string username)
        {
            if (IsBorrowed)
                throw new InvalidOperationException("Book is already borrowed.");

            if (!string.IsNullOrEmpty(ReservedByUser))
                throw new InvalidOperationException("Book is already reserved.");

            ReservedByUser = username;
        }

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

        // A string representation of the book.
        public override string ToString()
        {
            return $"ISBN: {ISBN}\nTitle: {Title}\nAuthor: {Author}\nPublished Year: {PublishedYear}\nGenre: {Genre}\nAvailable: {IsAvailable}\n";
        }
    }
}
