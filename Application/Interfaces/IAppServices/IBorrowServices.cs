using Application.DTOs.Borrow;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IAppServices
{
    public interface IBorrowServices
    {
        Task<bool> IsBannedCustomer(string customerId);
        bool CreateBorrowValidator(string customerId);
        Task<IReadOnlyList<ReadBorrowDto>> SearchWithCriteria(string customerName = null, string bookTitle = null, DateTime? date = null);
    }
}

