using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab5.Models;

namespace Lab5.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly Lab5.Models.ApplicationContext _context;

        public IndexModel(Lab5.Models.ApplicationContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; }

        public async Task OnGetAsync()
        {
            Order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Pizza).ToListAsync();
        }
    }
}
