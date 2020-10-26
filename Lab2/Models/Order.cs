using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.Models
{
    class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public Guid PizzaId { get; set; }

        [ForeignKey("PizzaId")]
        public Pizza Pizza { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public int Discount { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", new object[] { Id, ClientId, PizzaId, Date, Amount, Discount });
        }
    }
}
