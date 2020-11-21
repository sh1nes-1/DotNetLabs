using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Lab5.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, ApplicationContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<ClientAndPizzas> ClientsAndPizzas { get; set; }

        public void OnGet()
        {
            ClientsAndPizzas = _context.Orders
               .Join(_context.Clients, o => o.ClientId, c => c.Id,
                   (o, c) => new { PizzaId = o.PizzaId, FirstName = c.FirstName, LastName = c.LastName })
               .Join(_context.Pizzas, oc => oc.PizzaId, p => p.Id,
                   (oc, p) => new { Pizza = p.Name, FirstName = oc.FirstName, LastName = oc.LastName })
               .GroupBy(t => new { t.FirstName, t.LastName }, true)
               .Where(g => g.Count() >= 2)
               .Select(r => new ClientAndPizzas
               {
                   FirstName = r.Key.FirstName,
                   LastName = r.Key.LastName,
                   Pizzas = string.Join(", ", r.Select(q => q.Pizza))
               });
        }
    }
}
