using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Models
{
    public class Address
    {
        private string _streetName;
        private string _city;
        private string _state;
        private string _zipcode;

        public Address(string street, string city, string state, string zipcode)
        {
            _streetName = street;
            _city = city;
            _state = state;
            _zipcode = zipcode;
        }

        public void DisplayAddress()
        {
            Console.WriteLine($"{_streetName} {_city}, {_state} {_zipcode}");
        }
    }
}
