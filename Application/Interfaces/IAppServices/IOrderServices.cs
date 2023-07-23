using Application.DTOs.BookOrderDetails;
using Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IAppServices
{
    public interface IOrderServices
    {
        Task<IReadOnlyList<ReadBookOrderDetailsDto>> GetOrderByIdWithDetail(int orderId);
        Task<IReadOnlyList<ReadBookOrderDetailsDto>> SearchBookOrderDetails(int? orderId = null, string customerName = null, string bookTitle = null);

        Task<IReadOnlyList<ReadOrderDto>> SearchOrders(int? orderId = null, int? customerId = null, string customerName = null, decimal? totalPrice = null, DateTime? date = null);

        void DeletOrderAsync(int orderId);

        Task<bool> IsValidOrderId(string orderId);
        Task<bool> IsAvailableBook(string bookId, string bookQuantity);
        void DecreaseQuantity(string BookId, string Quantity);
    }
}
