using Application.DTOs.ReturnedOrder;
using Application.DTOs.ReturnOrderDetails;
using Application.Exceptions;
using Application.Interfaces.IAppServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructure.AppServices
{
    public class ReturnedOrderServices : IReturnedOrderServices
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public ReturnedOrderServices(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ReadReturnedOrderDto>> SearchReturnedOrders(int? originorderId = null, int? customerId = null, string? customerName = null, decimal? totalPrice = null, DateTime? returndate = null)
        {
            try
            {
                var result = await (from o in _context.ReturnedOrders
                                    join c in _context.Customers
                                    on o.CustomerId equals c.Id
                                    where c.CustomerName.Contains(customerName)
                                    || o.OriginOrderId == originorderId
                                    || c.Id == customerId
                                    || o.ReturnDate == returndate
                                    || o.TotalPrice == totalPrice
                                    select new ReadReturnedOrderDto()
                                    {
                                        Id = o.Id,
                                        OriginOrderId = o.OriginOrderId,
                                        CustomerId = o.CustomerId,
                                        CustomerName = c.CustomerName,
                                        ReturnDate = o.ReturnDate,
                                        TotalPrice = o.TotalPrice
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }
        }


        public async Task<IReadOnlyList<ReadReturnOrderDetailsDto>> SearchReturnedOrdersDetails(int? returnedorderId = null, int? bookId = null, string customerName = null, string bookTitle = null)
        {
            try
            {
                var query = _context.ReturnOrderDetails.Include(o => o.Order.Customer).Include(o => o.Book).AsQueryable();
                query = query.Where(o => o.ReturnedOrderId == returnedorderId
                                    || o.BookId == bookId
                                    || o.Order.Customer.CustomerName.Contains(customerName)
                                    || o.Book.BookTitle.Contains(bookTitle));

                var result = await query.ToListAsync();
                return _mapper.Map<IReadOnlyList<ReturnOrderDetails>, IReadOnlyList<ReadReturnOrderDetailsDto>>(result);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }
        }


        public bool IsInReturnInterval(DateTime returndate, DateTime orderdate)
        {
            var returndateonly = returndate.Date;
            var orderdateonly = orderdate.Date;
            var daysSinceOrder = (returndateonly - orderdateonly).Days;
            if (daysSinceOrder > 5)
                return false;
            return true;
        }

        public bool CheckQuantity(int returnedBookQuantity, int orderBookQuantity)
        {
            if(returnedBookQuantity > 0 && returnedBookQuantity <= orderBookQuantity)
                return true;
            return false;
        }

        public void IncreaseQuantity(int BookId, int quantity)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == BookId);
            book.Quantity += quantity;
            _context.SaveChanges();
        }

        public void DeleteReturnedOrderAsync(int returnedorderId)
        {
            ReturnedOrder returnedorder = _context.ReturnedOrders.Find(returnedorderId);

            _context.ReturnedOrders.Remove(returnedorder);
            _context.SaveChanges();

            var Books = _context.ReturnOrderDetails.Where(o => o.ReturnedOrderId == returnedorderId);
            foreach (var book in Books)
            {
                _context.ReturnOrderDetails.Remove(book);
            }
            _context.SaveChanges();
        }

        public void DecreaseAuthorProfits(int BookId, decimal Price, int Quantity)
        {
            var authorId = _context.Set<Book>().FirstOrDefault(x => x.Id == BookId).AuthorId;

            var author = _context.Set<Author>().FirstOrDefault(x => x.Id == authorId);

            var Profit = Price * 0.05m * Quantity;
            author.AuthorProfits -= Profit;

            _context.SaveChanges();
        }
    }
}
