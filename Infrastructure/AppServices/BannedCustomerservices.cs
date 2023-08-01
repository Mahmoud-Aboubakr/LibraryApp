using Application.DTOs.BannedCustomer;
using Application.Exceptions;
using Application.Interfaces.IAppServices;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AppServices
{
    public class BannedCustomerservices : IBannedCustomerServices
    {
        private readonly LibraryDbContext _context;
        public BannedCustomerservices(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<ReadBannedCustomerDto>> SearchForBannedCustomer(string EmpName = null, string CustomerName = null)
        {
            try
            {
                var result = await (from B in _context.BannedCustomers
                                    join E in _context.Employees
                                    on B.EmpId equals E.Id
                                    join C in _context.Customers
                                    on B.CustomerId equals C.Id
                                    where E.EmpName.Contains(EmpName)
                                    || C.CustomerName.Contains(CustomerName)
                                    select new ReadBannedCustomerDto()
                                    {
                                        Id = B.Id,
                                        EmpId = B.EmpId,
                                        EmpName = E.EmpName,
                                        BanDate = B.BanDate,
                                        CustomerId = B.CustomerId,
                                        CustomerName = C.CustomerName
                                    }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString());
            }
        }
    }
}
