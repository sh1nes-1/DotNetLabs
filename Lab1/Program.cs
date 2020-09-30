using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;

namespace Lab1
{
    static class Program
    {
        const string Host = "localhost";
        const string Database = "pizza_delivery";
        const string User = "root";
        const string Password = "";

        static readonly string[] Tables = new[] { "clients", "pizzas", "orders" };

        static MySqlConnection connection;

        static void CreateConnection()
        {
            connection = new MySqlConnection($"Database={Database};Datasource={Host};User={User};Password={Password}");
        }

        /* Operations for each one table */

        static void FillData(string table, string[] lines)
        {            
            foreach (string line in lines)
            {
                var command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO {table} VALUES ({line.Replace(";", ", ")})";
                command.ExecuteNonQuery();
            }
        }

        static void PrintTable(string table)
        {
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {table}";
            var reader = command.ExecuteReader();

            Console.WriteLine($"----------- Table {table} -----------");

            try
            {
                while (reader.Read())
                {
                    object[] values = new object[reader.FieldCount];
                    reader.GetValues(values);

                    string str = values.Aggregate((v1, v2) => $"{v1} | {v2}").ToString();
                    Console.WriteLine(str);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to print data from table {table}\nError details: {ex.Message}");
                return;
            }
            finally
            {
                reader?.Close();
            }

            Console.WriteLine();            
        }

        /* Operations with all tables */

        static void ClearTables()
        {
            foreach (string table in Tables.Reverse())
            {
                var command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM {table}";
                command.ExecuteNonQuery();
            }
        }

        static void FillTables()
        {
            foreach (string table in Tables)
            {
                try
                {
                    string[] lines = File.ReadAllLines($"{table}.csv");
                    FillData(table, lines);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to fill data in table {table}\nError details: {ex.Message}");
                }
            }
        }

        static void PrintTables()
        {
            foreach (string table in Tables)
            {
                PrintTable(table);
            }
        }        

        /* Main */

        static void PrintOrdersDetailed()
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM orders o JOIN clients c ON o.client_id = c.id";
            var reader = command.ExecuteReader();

            Console.WriteLine($"----------- Detailed Orders -----------");

            try
            {
                while (reader.Read())
                {
                    object[] values = new object[reader.FieldCount];
                    reader.GetValues(values);

                    string str = values.Aggregate((v1, v2) => $"{v1} | {v2}").ToString();
                    Console.WriteLine(str);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to print Detailed Orders\nError details: {ex.Message}");
                return;
            }
            finally
            {
                reader?.Close();
            }
        }

        static void PrintClientsCountThatHasAtLeastTwoOrders()
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM orders o " +
                                  "JOIN clients c ON o.client_id = c.id " +
                                  "GROUP BY client_id " +
                                  "HAVING COUNT(*) >= 2";

            var reader = command.ExecuteReader();

            int rowCount = 0;
            while (reader.Read())
            {
                ++rowCount;
            }

            reader.Close();

            Console.WriteLine($"\n\nCount of clients that have at least two orders: {rowCount}\n\n");
        }

        static void Main(string[] args)
        {
            CreateConnection();

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't connect to database!\nError details: {ex.Message}");
                return;
            }

            try
            {
                ClearTables();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't clear tables!\nError details: {ex.Message}");
                return;
            }

            FillTables();
            PrintTables();
            PrintOrdersDetailed();
            PrintClientsCountThatHasAtLeastTwoOrders();

            try
            {
                connection.Close();
            }
            catch
            {
            }

            Console.Read();
        }
    }
}
