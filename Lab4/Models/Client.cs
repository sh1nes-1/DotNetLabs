using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    public class Client
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Display(Name = "Прізвище")]
        public string LastName { get; set; }

        [Display(Name = "По батькові")]
        public string MiddleName { get; set; }

        [Display(Name = "Адреса")]
        public string Address { get; set; }

        [Display(Name = "Номер телефону")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public string FullName => $"{FirstName} {LastName} {MiddleName}";

        public override string ToString()
        {
            return string.Join(" | ", new object[] { Id, FirstName, LastName, MiddleName, Address, Phone });
        }
    }
}
