using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab5.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }
        [Display(Name = "Клієнт")]
        public Client Client { get; set; }

        public Guid PizzaId { get; set; }
        [Display(Name = "Піца")]
        public Pizza Pizza { get; set; }

        [Display(Name = "Дата замовлення")]
        public DateTime Date { get; set; }

        [Display(Name = "Кількість")]
        public int Amount { get; set; }

        [Display(Name = "Знижка")]
        public int Discount { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", new object[] { Id, ClientId, PizzaId, Date, Amount, Discount });
        }
    }
}
