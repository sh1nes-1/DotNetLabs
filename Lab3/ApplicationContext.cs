using Lab3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3
{
    class ApplicationContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Order> Orders { get; set; }

        const string Host = "localhost";
        const string Db = "pizza_delivery_2";
        const string User = "root";
        const string Password = "";

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql($"Database={Db};Datasource={Host};User={User};Password={Password}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(order => order.Client)
                .WithMany(o => o.Orders)
                .HasForeignKey(order => order.ClientId);

            modelBuilder.Entity<Order>()
                .HasOne(order => order.Pizza)
                .WithMany(o => o.Orders)
                .HasForeignKey(order => order.PizzaId);
        }
    }
}
