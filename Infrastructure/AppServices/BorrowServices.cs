using Application.DTOs;
using Application.Interfaces.IAppServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AppServices
{
    public class BorrowServices : IBorrowServices
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public BorrowServices(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> IsBannedCustomer(string customerId)
        {
            int id = int.Parse(customerId);
            return await _context.Set<BannedCustomer>().AnyAsync(x => x.CustomerId == id);
        }

        public bool CreateBorrowValidator(string customerId )
        {
            int id = int.Parse(customerId);
            var BorrowCount = _context.Borrows.Count(B => B.CustomerId == id && B.BorrowDate.Date == DateTime.Now.Date );
            if (BorrowCount >= 3 )
            {
                return false;
            }

            return true;
        }

        public async Task<IReadOnlyList<ReadBorrowDto>> SearchWithCriteria(string customerName = null, string bookTitle = null, DateTime? date = null)
        {
            var query = _context.Borrows.Include(b => b.Customer).Include(b => b.Book).AsQueryable();           

            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(b => b.Customer.CustomerName.Contains(customerName));
            }

            if (!string.IsNullOrEmpty(bookTitle))
            {
                query = query.Where(b => b.Book.BookTitle.Contains(bookTitle));
            }

            if (date.HasValue)
            {
                query = query.Where(b => b.ReturnDate.Date == date.Value.Date);
            }

            var result = await query.ToListAsync();
            return _mapper.Map<IReadOnlyList<Borrow>, IReadOnlyList<ReadBorrowDto>>(result);
        }

    }
}
