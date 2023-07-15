﻿using Domain.Entities;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class LibraryDbContextSeed
    {
        public static async Task SeedAsync(LibraryDbContext context)
        {
            if(!context.Authors.Any())
            {
                var authorsData = File.ReadAllText("../Persistence/DataSeeding/Authors.json");
                var authors = JsonSerializer.Deserialize<List<Author>>(authorsData);
                context.Authors.AddRange(authors);
            }

            if (!context.Publishers.Any())
            {
                var publishersData = File.ReadAllText("../Persistence/DataSeeding/Publishers.json");
                var publishers = JsonSerializer.Deserialize<List<Publisher>>(publishersData);
                await context.Publishers.AddRangeAsync(publishers);
            }

            if (!context.Books.Any())
            {
                var booksData = File.ReadAllText("../Persistence/DataSeeding/Books.json");
                var books = JsonSerializer.Deserialize<List<Book>>(booksData);
                await context.Books.AddRangeAsync(books);
            }

            if (!context.Customers.Any())
            {
                var customersData = File.ReadAllText("../Persistence/DataSeeding/Customers.json");
                var customers = JsonSerializer.Deserialize<List<Customer>>(customersData);
                await context.Customers.AddRangeAsync(customers);
            }


            //if (!context.BannedCustomers.Any())
            //{
            //    var bannedCustomersData = File.ReadAllText("../Persistence/DataSeeding/BannedCustomers.json");
            //    var bannedCustomers = JsonSerializer.Deserialize<List<BannedCustomer>>(bannedCustomersData);
            //    context.BannedCustomers.AddRange(bannedCustomers);
            //}


            if (!context.Orders.Any())
            {
                var ordersData = File.ReadAllText("../Persistence/DataSeeding/Orders.json");
                var orders = JsonSerializer.Deserialize<List<Order>>(ordersData);
                context.Orders.AddRange(orders);
            }

            if (!context.OrdersBooks.Any())
            {
                var ordersBooksData = File.ReadAllText("../Persistence/DataSeeding/OrderBooks.json");
                var ordersBooks = JsonSerializer.Deserialize<List<OrderBooks>>(ordersBooksData);
                context.OrdersBooks.AddRange(ordersBooks);
            }

            if (!context.Borrows.Any())
            {
                var borrowsData = File.ReadAllText("../Persistence/DataSeeding/Borrows.json");
                var borrows = JsonSerializer.Deserialize<List<Borrow>>(borrowsData);
                context.Borrows.AddRange(borrows);
            }

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();
        }
    }
}
