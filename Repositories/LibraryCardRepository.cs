using Library_Management_System.Models;

namespace Library_Management_System.Repositories
{
    public class LibraryCardRepository
    {
        // Dictionary to store library cards by username for quick lookup
        private readonly Dictionary<string, LibraryCard> libraryCards = new Dictionary<string, LibraryCard>();

        // Singleton instance
        private static LibraryCardRepository? _instance;

        // Private constructor to prevent direct instantiation
        private LibraryCardRepository() { }

        // Public property to access the singleton instance
        public static LibraryCardRepository Instance
        {
            get => _instance ??= new LibraryCardRepository();
        }

        /* Adds a library card to the repository for a specific user.
        *
        * This method adds the provided library card object to the dictionary,
        * ensuring that no duplicate cards exist for the same user.
        *
        * @param username The username associated with the library card.
        * @param card The LibraryCard object to be added.
        */
        public void AddLibraryCard(string username, LibraryCard card)
        {
            if (!libraryCards.ContainsKey(username))
            {
                libraryCards.Add(username, card);
            }
            else
            {
                Console.WriteLine("User already has a library card.");
            }
        }

        /* Removes a library card associated with a user from the repository.
        *
        * This method deletes the library card for the specified username,
        * if it exists in the repository.
        *
        * @param username The username associated with the library card to remove.
        */
        public void RemoveLibraryCard(string username)
        {
            if (libraryCards.ContainsKey(username))
            {
                libraryCards.Remove(username);
            }
            else
            {
                Console.WriteLine("This user doesn't exist.");
            }
        }

        /* Checks if a user has a library card in the repository.
        *
        * This method verifies the existence of a library card associated with the specified username.
        *
        * @param username The username to check for a library card.
        * @return True if the user has a library card; otherwise, false.
        */
        public bool ContainsUser(string username)
        {
            return libraryCards.ContainsKey(username);
        }

        /* Retrieves the library card associated with a specific user.
        *
        * This method searches the dictionary for the library card of the specified username.
        * If found, the library card is returned; otherwise, null is returned.
        *
        * @param username The username associated with the library card.
        * @return The LibraryCard object if found; otherwise, null.
        */
        public LibraryCard? GetCard(string username)
        {
            libraryCards.TryGetValue(username, out LibraryCard card);
            return card;
        }

        /* Displays all usernames that have library cards in the repository.
        *
        * This method iterates through all keys in the dictionary and prints the usernames.
        */
        public void DisplayUsers()
        {
            foreach (var name in libraryCards.Keys)
            {
                Console.WriteLine(name);
            }
        }
    }
}
