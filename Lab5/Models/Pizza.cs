using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab5.Models
{
    public class Pizza
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Display(Name = "Ціна")]
        public int Price { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", new object[] { Id, Name, Description, Price });
        }
    }
}
