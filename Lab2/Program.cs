using Lab2.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;

namespace Lab2
{
    class Program
    {
        const string DataPath = "Data";

        static List<T> ReadObjects<T>(string filename)
        {
            string json = File.ReadAllText($"{DataPath}\\{filename}");
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        static void ImportData(ApplicationContext context)
        {
            context.AddRange(ReadObjects<Client>("clients.json"));
            context.AddRange(ReadObjects<Pizza>("pizzas.json"));
            context.AddRange(ReadObjects<Order>("orders.json"));
            context.SaveChanges();
        }

        static void PrintTable<T>(string name, List<T> items) where T : class
        {
            Console.WriteLine($"\n------- {name} -------");
            items.ForEach(Console.WriteLine);
        }

        static void PrintData(ApplicationContext context)
        {
            PrintTable("Clients", context.Clients.ToList());
            PrintTable("Pizzas", context.Pizzas.ToList());
            PrintTable("Orders", context.Orders.ToList());
        }

        static void PrintAggregatedData(ApplicationContext context)
        {
            var data = context.Orders
                .Join(context.Clients, o => o.ClientId, c => c.Id,
                    (o, c) => new { PizzaId = o.PizzaId, FirstName = c.FirstName, LastName = c.LastName })
                .Join(context.Pizzas, oc => oc.PizzaId, p => p.Id,
                    (oc, p) => new { Pizza = p.Name, FirstName = oc.FirstName, LastName = oc.LastName })
                .ToList()
                .GroupBy(t => new { t.FirstName, t.LastName })
                .Where(g => g.Count() >= 2);

            Console.WriteLine("\n------- Aggregated data -------");
            foreach (var el in data)
            {
                string[] pizzas = el.Select(q => q.Pizza).ToArray();

                Console.WriteLine($"'{el.Key.FirstName} {el.Key.LastName}' " +
                    $"ordered pizzas [{string.Join(", ", pizzas)}], " +
                    $"Count: {el.Count()}");
            }
        }

        static void DeleteData(ApplicationContext context)
        {
            context.Orders.RemoveRange(context.Orders);
            context.Clients.RemoveRange(context.Clients);
            context.Pizzas.RemoveRange(context.Pizzas);
            context.SaveChanges();
        }

        static void Main(string[] args)
        {
            using (var context = new ApplicationContext())
            {
                ImportData(context);
                PrintData(context);
                PrintAggregatedData(context);
                DeleteData(context);
            }
        }
    }
}
