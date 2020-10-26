using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.Models
{
    class Pizza
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        [InverseProperty("Pizza")]
        public virtual ICollection<Order> Orders { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", new object[] { Id, Name, Description, Price });
        }
    }
}
