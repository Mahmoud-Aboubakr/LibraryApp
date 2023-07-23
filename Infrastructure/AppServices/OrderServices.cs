using Application.DTOs.BookOrderDetails;
using Application.DTOs.Order;
using Application.Interfaces.IAppServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AppServices
{
    public class OrderServices : IOrderServices
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public OrderServices(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> IsValidOrderId(string orderId)
        {
            int id = int.Parse(orderId);
            return await _context.Set<Order>().AnyAsync(x => x.Id == id);
        }

        public async Task<bool> IsAvailableBook(string bookId , string bookQuantity)
        {
            int id = int.Parse(bookId);
            int quantity = int.Parse(bookQuantity);
            return await _context.Set<Book>().AnyAsync(x => x.Id == id && x.Quantity >= quantity);
        }

        public void DecreaseQuantity(string BookId , string Quantity)
        {
            int id = int.Parse(BookId);
            int quantity = int.Parse(Quantity);
            var book = _context.Books.FirstOrDefault(x => x.Id == id);
            book.Quantity -= quantity;
            _context.SaveChanges();
        }
   
        public void DeletOrderAsync(int orderId)
        {
            Order order = _context.Orders.Find(orderId);

            _context.Orders.Remove(order);
            _context.SaveChanges();

            var Books = _context.BookOrderDetails.Where(o => o.OrderId == orderId);
            foreach (var book in Books)
            {
                _context.BookOrderDetails.Remove(book);
            }
            _context.SaveChanges();

        }

        public async Task<IReadOnlyList<ReadBookOrderDetailsDto>> GetOrderByIdWithDetail(int orderId)
        {
            var query = _context.BookOrderDetails.Include(b => b.Order.Customer).Include(b => b.Book).AsQueryable();
            query = query.Where(b => b.OrderId == orderId);

            var result = await query.ToListAsync();
            return _mapper.Map<IReadOnlyList<BookOrderDetails>, IReadOnlyList<ReadBookOrderDetailsDto>>(result);
        }

        public async Task<IReadOnlyList<ReadBookOrderDetailsDto>> SearchBookOrderDetails(int? orderId = null, string customerName = null, string bookTitle = null)
        {
            var query = _context.BookOrderDetails.Include(b => b.Order.Customer).Include(b => b.Book).AsQueryable();

            if (orderId.HasValue)
            {
                query = query.Where(b => b.OrderId == orderId.Value);
            }

            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(b => b.Order.Customer.CustomerName.Contains(customerName));
            }

            if (!string.IsNullOrEmpty(bookTitle))
            {
                query = query.Where(b => b.Book.BookTitle.Contains(bookTitle));
            }

            var result = await query.ToListAsync();
            return _mapper.Map<IReadOnlyList<BookOrderDetails>, IReadOnlyList<ReadBookOrderDetailsDto>>(result);
        }

        public async Task<IReadOnlyList<ReadOrderDto>> SearchOrders(int? orderId = null, int? customerId = null, string customerName = null , decimal? totalPrice = null, DateTime? date = null)
        {
            var query = _context.Orders.Include(b => b.Customer).AsQueryable();

            if (orderId.HasValue)
            {
                query = query.Where(b => b.Id == orderId.Value);
            }

            if (customerId.HasValue)
            {
                query = query.Where(b => b.CustomerId == customerId.Value);
            }

            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(b => b.Customer.CustomerName.Contains(customerName));
            }

            if (totalPrice.HasValue)
            {
                query = query.Where(b => b.TotalPrice >= totalPrice);
            }

            if (date.HasValue)
            {
                query = query.Where(b => b.OrderDate.Date >= date.Value.Date);
            }

            var result = await query.ToListAsync();
            return _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<ReadOrderDto>>(result);
        }
    }
}
