using Application.DTOs;
using Application.Interfaces;
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
    public class OrderServices : IOrderServices
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public OrderServices(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
