using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lab4.Models;

namespace Lab4.Controllers
{
    public class HomeController : Controller
    {        
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Orders
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

            return View(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
