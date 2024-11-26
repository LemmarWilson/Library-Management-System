using Library_Management_System.Repositories;
using Library_Management_System.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Library_Management_System.Models
{
    public class LibraryCard
    {
        static int id = 100;

        const int MAXBOOKS = 5;
        private int _userId;
        private string _firstName;
        private string _lastName;
        private Address _address;
        private DateTime _issueDate;
        private DateTime _renewalDate;
        private List<string> _borrowedBooks = new List<string>();

        public LibraryCard() { }
        public LibraryCard(string firstName, string lastName, Address address)
        {
            _userId = id;
            FirstName = firstName;
            LastName = lastName;
            _address = address;
            _issueDate = DateTime.Now;
            _renewalDate = DateTime.Now.AddYears(1);
            id++;
        }

        public int UserId
        {
            get => _userId;
        }
        public string FirstName
        {
            get => _firstName;
            set => _firstName = !string.IsNullOrEmpty(value) ? value : _firstName;
        }
        public string LastName
        {
            get => _firstName;
            set => _firstName = !string.IsNullOrEmpty(value) ? value : _firstName;
        }
        public Address Address
        {
            get => _address;
        }
        public DateTime IssueDate
        {
            get => _issueDate;
        }
        public DateTime RenewalDate
        {
            get => _renewalDate;
        }
        public int Count
        {
            get => _borrowedBooks.Count;
        }

        public void BorrowBook(string isbn)
        {
            _borrowedBooks.Add(isbn);
        }
        public bool ReturnBook(string isbn)
        {
            if (_borrowedBooks.Contains(isbn))
            {
                _borrowedBooks.Remove(isbn);
                Console.WriteLine("{0} has been returned", isbn);
                return true;
            }
            Console.WriteLine("You have not borrowed this book");
            return false;
        }
        public bool IsActive()
        {
            return IssueDate < RenewalDate;
        }
        public void RenewCard()
        {
            _issueDate = DateTime.Now;
            _renewalDate = _issueDate.AddYears(1);
        }
        public void DisplayBooks()
        {
            Console.WriteLine("\nYou currently have: ");
            foreach (var book in _borrowedBooks)
            {
                Book borrowed = BookRepository.Instance.GetBookByISBN(book);
                Console.WriteLine("\tTitle: " + borrowed.Title);
                Console.WriteLine("\tAuthor: " + borrowed.Author);
                Console.WriteLine();
            }
        }
    }
}
