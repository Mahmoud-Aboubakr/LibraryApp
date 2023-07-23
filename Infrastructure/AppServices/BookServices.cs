using Application.DTOs.Book;
using Application.Interfaces.IAppServices;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Infrastructure.AppServices
{
    public class BookServices : IBookServices
    {
        private readonly LibraryDbContext _context;
        public BookServices(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<ReadBookDto>> SearchBookDataWithDetail(string bookTitle = null, string authorName = null, string publisherName = null)
        {
            var result = await (from b in _context.Books
                                join a in _context.Authors
                                on b.AuthorId equals a.Id
                                join p in _context.Publishers
                                on b.PublisherId equals p.Id
                                where b.BookTitle.Contains(bookTitle)
                                || a.AuthorName.Contains(authorName)
                                || p.PublisherName.Contains(publisherName)
                                select new ReadBookDto()
                                {
                                    Id = b.Id,
                                    BookTitle = b.BookTitle,
                                    Price = b.Price,
                                    Quantity = b.Quantity,
                                    AuthorName = a.AuthorName,
                                    PublisherName = p.PublisherName
                                }).ToListAsync();
            return result;
        }
    }
}
