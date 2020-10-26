using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3
{
    public class HtmlWriter
    {
        const string TEMPLATES_PATH = "./wwwroot/templates/";

        /* Utility functions */

        private static string GetTemplate(string filename)
        {
            return File.ReadAllText(Path.Combine(TEMPLATES_PATH, filename));
        }

        private static string RenderTemplate(string template, Dictionary<string, string> parameters)
        {
            return parameters.Aggregate(template, (temp, pair) => temp.Replace($"@{pair.Key}", pair.Value));
        }

        public static async Task WritePage(HttpContext httpContext, string title, string body)
        {
            string page = RenderTemplate(GetTemplate("app.html"), new Dictionary<string, string>()
            {
                { "title", title },
                { "body", body },
            });

            await httpContext.Response.WriteAsync(page);
        }

        /* Pages */

        public static async Task WriteMainPage(HttpContext httpContext)
        {
            using (var context = new ApplicationContext())
            {
                var data = context.Orders
                    .Join(context.Clients, o => o.ClientId, c => c.Id,
                        (o, c) => new { PizzaId = o.PizzaId, FirstName = c.FirstName, LastName = c.LastName })
                    .Join(context.Pizzas, oc => oc.PizzaId, p => p.Id,
                        (oc, p) => new { Pizza = p.Name, FirstName = oc.FirstName, LastName = oc.LastName })
                    .ToList()
                    .GroupBy(t => new { t.FirstName, t.LastName })
                    .Where(g => g.Count() >= 2);

                string clientOrdersTable = string.Empty;
                foreach (var el in data)
                {
                    string[] pizzas = el.Select(q => q.Pizza).ToArray();

                    clientOrdersTable += RenderTemplate(GetTemplate("client_order.html"), new Dictionary<string, string>()
                    {
                        { "FirstName", el.Key.FirstName },
                        { "LastName", el.Key.LastName },
                        { "Pizzas", string.Join(", ", pizzas) },
                    });
                }

                string mainPage = RenderTemplate(GetTemplate("main.html"), new Dictionary<string, string>()
                {
                    { "client_orders", clientOrdersTable }
                });

                await WritePage(httpContext, "Main Page", mainPage);
            }
        }

        public static async Task WriteClientsPage(HttpContext httpContext)
        {
            string clientTemplate = GetTemplate("client.html");
            string clientsTable = string.Empty;

            using (var context = new ApplicationContext())
            {
                foreach (var client in context.Clients)
                {
                    clientsTable += RenderTemplate(clientTemplate, new Dictionary<string, string>()
                    {
                        { "FirstName", client.FirstName },
                        { "LastName", client.LastName },
                        { "MiddleName", client.MiddleName },
                        { "Address", client.Address },
                        { "Phone", client.Phone },
                    });
                }
            }

            string clientsPage = RenderTemplate(GetTemplate("clients.html"), new Dictionary<string, string>()
            {
                { "clients", clientsTable }
            });

            await WritePage(httpContext, "Clients", clientsPage);
        }

        public static async Task WritePizzasPage(HttpContext httpContext)
        {
            string pizzaTemplate = GetTemplate("pizza.html");
            string pizzasTable = string.Empty;

            using (var context = new ApplicationContext())
            {
                foreach (var pizza in context.Pizzas)
                {
                    pizzasTable += RenderTemplate(pizzaTemplate, new Dictionary<string, string>()
                    {
                        { "Name", pizza.Name },
                        { "Description", pizza.Description },
                        { "Price", pizza.Price.ToString() },
                    });
                }
            }

            string clientsPage = RenderTemplate(GetTemplate("pizzas.html"), new Dictionary<string, string>()
            {
                { "pizzas", pizzasTable }
            });

            await WritePage(httpContext, "Pizzas", clientsPage);
        }
    }
}
