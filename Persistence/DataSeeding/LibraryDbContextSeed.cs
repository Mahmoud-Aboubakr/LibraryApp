using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Persistence.Context;
using System.Text.Json;

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

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();
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

            if (!context.Borrows.Any())
            {
                var borrowsData = File.ReadAllText("../Persistence/DataSeeding/Borrows.json");
                var borrows = JsonSerializer.Deserialize<List<Borrow>>(borrowsData);
                context.Borrows.AddRange(borrows);
            }

            if (!context.Orders.Any())
            {
                var ordersData = File.ReadAllText("../Persistence/DataSeeding/Orders.json");
                var orders = JsonSerializer.Deserialize<List<Order>>(ordersData);
                context.Orders.AddRange(orders);
            }

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();
            #endregion

            #region Phase 3 to run
            if (!context.BookOrderDetails.Any())
            {
                var bookOrderData = File.ReadAllText("../Persistence/DataSeeding/BookOrderDetails.json");
                var bookOrder = JsonSerializer.Deserialize<List<BookOrderDetails>>(bookOrderData);
                context.BookOrderDetails.AddRange(bookOrder);
            }

            if (!context.ReturnedOrders.Any())
            {
                var ReturnedOrdersData = File.ReadAllText("../Persistence/DataSeeding/ReturnedOrders.json");
                var returnedOrders = JsonSerializer.Deserialize<List<ReturnedOrder>>(ReturnedOrdersData);
                context.ReturnedOrders.AddRange(returnedOrders);
            }

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();
            #endregion

            #region Phase 4 to run
            if (!context.ReturnOrderDetails.Any())
            {
                var ReturnOrderDetailsData = File.ReadAllText("../Persistence/DataSeeding/ReturnOrderDetails.json");
                var returnOrderDetails = JsonSerializer.Deserialize<List<ReturnOrderDetails>>(ReturnOrderDetailsData);
                context.ReturnOrderDetails.AddRange(returnOrderDetails);
            }

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();
            #endregion
        }

        public static async Task SeedDemoUserAndRoles(LibraryDbContext context, UserManager<ApplicationUser> _userManager)
        {
            if (!context.Roles.Any())
            {
                var rolesData = File.ReadAllText("../Persistence/DataSeeding/Roles.json");
                var roles = JsonSerializer.Deserialize<List<IdentityRole>>(rolesData);
                context.Roles.AddRange(roles);
            }

            if (!context.Users.Any())
            {
                var usersData = File.ReadAllText("../Persistence/DataSeeding/Users.json");
                var users = JsonSerializer.Deserialize<List<ApplicationUser>>(usersData);
                context.Users.AddRange(users);
            }

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();

            var user = context.Users.FirstOrDefault(u => u.UserName == "string");
            await _userManager.AddToRoleAsync(user, "Manager");
        }
    }
}