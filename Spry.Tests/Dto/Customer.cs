using System;

namespace Spry.Tests.Dto
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public CustomerAddress Address { get; set; }
    }
}
