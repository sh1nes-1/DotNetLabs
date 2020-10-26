using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab3.Models
{
    class Client
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        [InverseProperty("Client")]
        public virtual ICollection<Order> Orders { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", new object[] { Id, FirstName, LastName, MiddleName, Address, Phone });
        }
    }
}
