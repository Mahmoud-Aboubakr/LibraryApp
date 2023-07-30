using Application.DTOs.ReturnedOrder;
using Application.DTOs.ReturnOrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IAppServices
{
    public interface IReturnedOrderServices
    {
        Task<IReadOnlyList<ReadReturnedOrderDto>> SearchReturnedOrders(int? originorderId = null, int? customerId = null, string customerName = null, decimal? totalPrice = null, DateTime? returndate = null);

        Task<IReadOnlyList<ReadReturnOrderDetailsDto>> SearchReturnedOrdersDetails(int? returnedorderId = null, int? bookId = null, string customerName = null, string bookTitle = null);

        bool IsInReturnInterval(DateTime returndate, DateTime orderdate);

        void IncreaseQuantity(int BookId, int quantity);

        bool CheckQuantity(int returnedBookQuantity, int orderBookQuantity);

        void DeleteReturnedOrderAsync(int returnedorderId);

        public void DecreaseAuthorProfits(int BookId, decimal Price, int Quantity);
    }
}
