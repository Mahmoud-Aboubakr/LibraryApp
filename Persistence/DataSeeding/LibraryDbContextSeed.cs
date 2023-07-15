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
                context.Publishers.AddRange(publishers);
            }

            if (!context.Books.Any())
            {
                var booksData = File.ReadAllText("../Persistence/DataSeeding/Books.json");
                var books = JsonSerializer.Deserialize<List<Book>>(booksData);
                context.Books.AddRange(books);
            }

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();
        }
    }
}
