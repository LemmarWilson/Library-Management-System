using Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Repositories
{
    public class LibraryCardRepository
    {
        //creates singleton
        private readonly Dictionary<string, LibraryCard> libraryCards = new Dictionary<string, LibraryCard>();
        private static LibraryCardRepository? _instance;
        private LibraryCardRepository() { }

        //property to access singleton instance
        public static LibraryCardRepository Instance
        {
            get => _instance ??= new LibraryCardRepository();
        }

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
        public void RemoveLibraryCard(string username)
        {
            if (libraryCards.ContainsKey(username))
            {
                libraryCards.Remove(username);
            }
            else
            {
                Console.WriteLine("This user doesn't exist");
            }
        }
        public bool ContainsUser(string username)
        {
            return libraryCards.ContainsKey(username);
        }
        public LibraryCard GetCard(string username)
        {
            LibraryCard libraryCard = null;
            if (libraryCards.TryGetValue(username, out LibraryCard card))
            {
                libraryCard = card;
            }
            return libraryCard;
            
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
