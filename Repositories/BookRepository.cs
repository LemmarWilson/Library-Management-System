using System.Collections.Generic;
using Library_Management_System.Models;

namespace Library_Management_System.Repositories
{
    public class BookRepository
    {
        // Dictionary to store books by their ISBN for quick lookup
        private readonly Dictionary<string, Book> books = new Dictionary<string, Book>();

        // Singleton instance
        private static BookRepository? _instance;

        // Private constructor to prevent direct instantiation
        private BookRepository() { }

        // Public method to get the singleton instance
        public static BookRepository Instance => _instance ??= new BookRepository();

        /* Adds a new book to the repository.
        *
        * This method adds the provided book object to the dictionary.
        * It assumes that the caller has already performed any necessary validations.
        *
        * @param book The Book object to be added to the repository.
        */
        public void AddBook(Book book)
        {
            books[book.ISBN] = book;
        }

        /* Retrieves a book by its ISBN.
        *
        * This method searches the dictionary for a book with the specified ISBN.
        * If found, the book is returned; otherwise, null is returned.
        *
        * @param isbn The ISBN of the book to retrieve.
        * @return The Book object if found; otherwise, null.
        */
        public Book? GetBookByISBN(string isbn)
        {
            books.TryGetValue(isbn, out var book);
            return book;
        }

        /* Retrieves books by their title.
        *
        * This method searches the dictionary for books that contain the specified title.
        * The search is case-insensitive.
        *
        * @param title The title or part of the title of the books to retrieve.
        * @return A list of Book objects that match the title search.
        */
        public List<Book> GetBooksByTitle(string title)
        {
            title = title.ToLower();
            var matchingBooks = new List<Book>();

            foreach (var book in books.Values)
            {
                if (book.Title.ToLower().Contains(title))
                {
                    matchingBooks.Add(book);
                }
            }

            return matchingBooks;
        }

        /* Retrieves books by their author.
        *
        * This method searches the dictionary for books written by the specified author.
        * The search is case-insensitive.
        *
        * @param author The author or part of the author's name of the books to retrieve.
        * @return A list of Book objects that match the author search.
        */
        public List<Book> GetBooksByAuthor(string author)
        {
            author = author.ToLower();
            var matchingBooks = new List<Book>();

            foreach (var book in books.Values)
            {
                if (book.Author.ToLower().Contains(author))
                {
                    matchingBooks.Add(book);
                }
            }

            return matchingBooks;
        }

        /* Retrieves all books in the repository.
        *
        * This method returns all books stored in the dictionary.
        *
        * @return A list of all Book objects in the repository.
        */
        public List<Book> GetAllBooks()
        {
            return new List<Book>(books.Values);
        }

        /* Deletes a book by its ISBN.
        *
        * This method searches for a book with the specified ISBN and removes it from the dictionary if found.
        *
        * @param isbn The ISBN of the book to delete.
        */
        public void DeleteBook(string isbn)
        {
            books.Remove(isbn);
        }

        /* Updates the information of a book by its ISBN.
        *
        * This method searches for a book with the specified ISBN and updates its details.
        *
        * @param isbn The ISBN of the book to update.
        * @param title The new title of the book.
        * @param author The new author of the book.
        * @param publishedYear The new published year of the book.
        * @param genre The new genre of the book.
        */
        public void UpdateBook(string isbn, string title, string author, int publishedYear, string genre)
        {
            if (books.TryGetValue(isbn, out var book))
            {
                book.UpdateBookInfo(title, author, publishedYear, genre);
            }
        }

    }
}
