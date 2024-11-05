using Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Repositories
{
    public class LibraryCardRepository
    {
        private readonly Dictionary<string, LibraryCard> libraryCards = new Dictionary<string, LibraryCard>();
        private static LibraryCardRepository? _instance;
        private LibraryCardRepository() { }
        public static LibraryCardRepository Instance => _instance ??= new LibraryCardRepository();

        public void AddLibraryCard(string username, LibraryCard card)
        {
            if (!libraryCards.ContainsKey(username))
            {
                libraryCards.Add(username, card);
            }
            else
            {
                Console.WriteLine("User already has a library card");
            }
        }
        public void DisplayUsers()
        {
            foreach (var name in libraryCards.Keys)
            {
                Console.WriteLine(name);
            }
        }
    }
}
