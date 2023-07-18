using Domain.Entities;
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
            #region Phase 1 to run
            if (!context.Authors.Any())
            {
                var authorsData = File.ReadAllText("../Persistence/DataSeeding/Authors.json");
                var authors = JsonSerializer.Deserialize<List<Author>>(authorsData);
                context.Authors.AddRange(authors);
            }

            if (!context.Publishers.Any())
            {
                var publishersData = File.ReadAllText("../Persistence/DataSeeding/Publishers.json");
                var publishers = JsonSerializer.Deserialize<List<Publisher>>(publishersData);
                context.Publishers.AddRange(publishers);
            }

            if (!context.Customers.Any())
            {
                var customersData = File.ReadAllText("../Persistence/DataSeeding/Customers.json");
                var customers = JsonSerializer.Deserialize<List<Customer>>(customersData);
                context.Customers.AddRange(customers);
            }

            if (!context.Employees.Any())
            {
                var EmployeesData = File.ReadAllText("../Persistence/DataSeeding/Employees.json");
                var employees = JsonSerializer.Deserialize<List<Employee>>(EmployeesData);
                context.Employees.AddRange(employees);
            }
            #endregion
            #region Phase 2 to run
            if (!context.Books.Any())
            {
                var booksData = File.ReadAllText("../Persistence/DataSeeding/Books.json");
                var books = JsonSerializer.Deserialize<List<Book>>(booksData);
                context.Books.AddRange(books);
            }

            if (!context.Attendence.Any())
            {
                var AttendencesData = File.ReadAllText("../Persistence/DataSeeding/Attendence.json");
                var attendences = JsonSerializer.Deserialize<List<Attendence>>(AttendencesData);
                context.Attendence.AddRange(attendences);
            }

            if (!context.Vacations.Any())
            {
                var VacationsData = File.ReadAllText("../Persistence/DataSeeding/Vacations.json");
                var vacations = JsonSerializer.Deserialize<List<Vacation>>(VacationsData);
                context.Vacations.AddRange(vacations);
            }

            if (!context.Payrolls.Any())
            {
                var PayrollsData = File.ReadAllText("../Persistence/DataSeeding/Payrolls.json");
                var payrolls = JsonSerializer.Deserialize<List<Payroll>>(PayrollsData);
                context.Payrolls.AddRange(payrolls);
            }

            if (!context.BannedCustomers.Any())
            {
                var bannedCustomersData = File.ReadAllText("../Persistence/DataSeeding/BannedCustomers.json");
                var bannedCustomers = JsonSerializer.Deserialize<List<BannedCustomer>>(bannedCustomersData);
                context.BannedCustomers.AddRange(bannedCustomers);
            }

            if (!context.Orders.Any())
            {
                var ordersData = File.ReadAllText("../Persistence/DataSeeding/Orders.json");
                var orders = JsonSerializer.Deserialize<List<Order>>(ordersData);
                context.Orders.AddRange(orders);
            }

            if (!context.Borrows.Any())
            {
                var borrowsData = File.ReadAllText("../Persistence/DataSeeding/Borrows.json");
                var borrows = JsonSerializer.Deserialize<List<Borrow>>(borrowsData);
                context.Borrows.AddRange(borrows);
            }
            #endregion
            #region Phase 3 to run
            if (!context.BookOrderDetails.Any())
            {
                var bookOrderData = File.ReadAllText("../Persistence/DataSeeding/BookOrderDetails.json");
                var bookOrder = JsonSerializer.Deserialize<List<BookOrderDetails>>(bookOrderData);
                context.BookOrderDetails.AddRange(bookOrder);
            }
            #endregion

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();
        }
    }
}
