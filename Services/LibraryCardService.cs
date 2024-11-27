using Library_Management_System.Models;
using Library_Management_System.Repositories;

namespace Library_Management_System.Services
{
    public class LibraryCardService
    {
        /* Prompts the user for address details and creates an Address object.
        *
        * This method interacts with the user to input their street, city, state, and zipcode.
        * It validates each input to ensure no null or empty values are provided.
        *
        * @return An Address object containing the entered address details.
        */
        private static Address CreateAddress()
        {
            string street, city, state, zipcode;

            // Prompt for and validate street name
            do
            {
                Console.Write("Enter your street name: ");
                street = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(street))
                {
                    Console.WriteLine("Street name cannot be empty. Please try again.");
                }
            } while (string.IsNullOrEmpty(street));

            // Prompt for and validate city
            do
            {
                Console.Write("Enter your city: ");
                city = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(city))
                {
                    Console.WriteLine("City cannot be empty. Please try again.");
                }
            } while (string.IsNullOrEmpty(city));

            // Prompt for and validate state
            do
            {
                Console.Write("Enter your state: ");
                state = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(state))
                {
                    Console.WriteLine("State cannot be empty. Please try again.");
                }
            } while (string.IsNullOrEmpty(state));

            // Prompt for and validate zipcode
            do
            {
                Console.Write("Enter your zipcode: ");
                zipcode = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(zipcode))
                {
                    Console.WriteLine("Zipcode cannot be empty. Please try again.");
                }
            } while (string.IsNullOrEmpty(zipcode));

            // Create and return the Address object
            return new Address(street, city, state, zipcode);
        }

        /* Creates a library card for a specific user.
        *
        * This method prompts the user for their first name, last name, and address.
        * It validates the input to ensure no null or empty values are provided.
        * Then, it creates a LibraryCard object and adds it to the repository for the specified username.
        *
        * @param username The username to associate with the created library card.
        */
        public static void CreateCard(string username)
        {
            string firstName, lastName;

            // Prompt for and validate first name
            do
            {
                Console.Write("Enter your first name: ");
                firstName = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(firstName))
                {
                    Console.WriteLine("First name cannot be empty. Please try again.");
                }
            } while (string.IsNullOrEmpty(firstName));

            // Prompt for and validate last name
            do
            {
                Console.Write("Enter your last name: ");
                lastName = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(lastName))
                {
                    Console.WriteLine("Last name cannot be empty. Please try again.");
                }
            } while (string.IsNullOrEmpty(lastName));

            // Get address details
            Address userAddress = CreateAddress();

            // Create a LibraryCard object
            LibraryCard? libraryCard = new LibraryCard(firstName, lastName, userAddress);

            // Add the library card to the repository
            LibraryCardRepository.Instance.AddLibraryCard(username, libraryCard);
            Console.WriteLine("Library card registered successfully.");
        }
    }
}