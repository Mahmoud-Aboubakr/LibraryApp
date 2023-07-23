using Application.DTOs.Customer;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IAppServices
{
    public interface ICustomerServices
    {
        Task<IReadOnlyList<ReadCustomerDto>> SearchWithCriteria(string Name = null, string PhoneNumber = null);
    }
}
