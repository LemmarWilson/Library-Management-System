using Library_Management_System.Models;
using Library_Management_System.Repositories;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Services
{
    public class LibraryCardService
    {
        private static Address GetAddress()
        {
            string street, city, state, zipcode;
            Console.Write("Enter your street name: ");
            street = Console.ReadLine();

            Console.Write("Enter your city: ");
            city = Console.ReadLine();

            Console.Write("Enter your state: ");
            state = Console.ReadLine();

            Console.Write("Enter your zipcode: ");
            zipcode = Console.ReadLine();

            Address userAddress = new Address(street, city, state, zipcode);

            return userAddress;
        }
        public static LibraryCard CreateCard()
        {
            return null;
        }
        public static void CreateCard(string username)
        {
            Console.Write("Enter your first name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter your last name: ");
            string lastName = Console.ReadLine();

            Address userAddress = GetAddress();

            LibraryCard? libraryCard = new LibraryCard(firstName, lastName, userAddress);

            LibraryCardRepository.Instance.AddLibraryCard(username, libraryCard);
        }
        //public static DeleteCard(string username)
        //{
            
        //}
    }
}
