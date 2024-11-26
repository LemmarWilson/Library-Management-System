using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace Library_Management_System.Models
{
    public class Password
    {
        private string hashPassword;
        public bool VerifyPassword(string checkPassword)
        {
            checkPassword = checkPassword.Replace(" ", ""); //remove all spacing in front, back & possibly in between???
            string hashCheckPassword = HashPassword(checkPassword);
            return hashPassword == checkPassword;

        }
        public static string InputPassword()
        {
            Console.Write("Please give a password: ");
            return Console.ReadLine();
        }

        public string GetPassword()
        {
            return hashPassword;
        }
        public void SetPassword(string newPassword)
        {
            newPassword = newPassword.Replace(" ", "");

           const string SPEACIAL_CHARACTERS = "!@#$%^&*:;.?<>_-+=";

            bool upper = false;
            bool lower = false;
            bool num = false;
            bool symbol = false;

            if (newPassword.Length >= 15)
            {
                throw new ArgumentException("Error password must be less than 15 characters");
            }
            else if (newPassword.Length <= 6)
            {
                throw new ArgumentException("Error password must be more than 6 characters");
            }

            foreach (char c in newPassword)
            {
                if (char.IsUpper(c))
                {
                    upper = true;
                }
                else if (char.IsLower(c))
                {
                    lower = true;
                }
                else if (char.IsNumber(c))
                {
                    num = true;
                }
                else if (SPEACIAL_CHARACTERS.Contains(c))
                {
                    symbol = true;
                }
            }

            if (!upper)
            {
                throw new ArgumentException("Needs one upper case letter");
            }
            else if (!lower)
            {
                throw new ArgumentException("Needs one lower case letter");
            }
            else if (!num)
            {
                throw new ArgumentException("Needs one number value");
            }
            else if (!symbol)
            {
                throw new ArgumentException("Needs one special character");
            }

            hashPassword = HashPassword(newPassword);

        }
        public static string HashPassword(string newPassword)
        {
            //Console.WriteLine("Password Received: {}", newPassword);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(newPassword);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                StringBuilder hashString = new StringBuilder();

                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }
                //Console.WriteLine("Hashed Password: {}",hashString.ToString());
                return hashString.ToString();
            }
        }
    }
}
