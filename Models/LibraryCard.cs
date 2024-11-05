using Library_Management_System.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

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

        public void BorrowBook(string bookTitle)
        {
            if (_borrowedBooks.Count < MAXBOOKS)
            {
                // check if the book is available before adding
                _borrowedBooks.Add(bookTitle);
                // change book object to false
                Console.WriteLine("You have checked out {0}", bookTitle);
            }
            else
            {
                Console.WriteLine("You can only borrow {0} books at a time", MAXBOOKS);
            }
        }
        public void ReturnBook(string bookTitle)
        {
            if (_borrowedBooks.Contains(bookTitle))
            {
                // set book object to true
                _borrowedBooks.Remove(bookTitle);
                Console.WriteLine("{0} has been returned", bookTitle);
            }
            else
            {
                Console.WriteLine("{0} is not part of your collection", bookTitle);
            }
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
    }

    //public class Test
    //{
    //    private static readonly LibraryCardRepository libraryCardRepository = LibraryCardRepository.Instance;

    //    static void Main()
    //    {
    //        Address a1 = new Address("464 st name", "City", "State", "90908");
    //        Address a2 = new Address("686 st name", "City", "State", "90210");

    //        LibraryCard libraryCard = new LibraryCard("Mike", "Harold", a1);
    //        LibraryCard newCard = new LibraryCard("Mike", "Harold", a2);

    //        libraryCard.Address.DisplayAddress();
    //        newCard.Address.DisplayAddress();

    //        libraryCardRepository.AddLibraryCard("mikr123", libraryCard);
    //        libraryCardRepository.AddLibraryCard("mpoyy", newCard);

    //        libraryCardRepository.DisplayUsers();
    //    }
    //}
}
