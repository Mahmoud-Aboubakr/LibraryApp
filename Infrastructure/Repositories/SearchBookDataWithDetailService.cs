using Application.DTOs;
using Domain.Entities;
using Infrastructure.AppServicesContracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SearchBookDataWithDetailService : ISearchBookDataWithDetailService
    {
        private readonly LibraryDbContext _context;
        public SearchBookDataWithDetailService(LibraryDbContext context)
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
