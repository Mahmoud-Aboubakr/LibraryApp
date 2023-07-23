using Application.DTOs.Customer;
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
    public class CustomerServices : ICustomerServices
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public CustomerServices(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<ReadCustomerDto>> SearchWithCriteria(string Name = null, string PhoneNumber = null)
        {
            var query = _context.Set<Customer>().AsQueryable();

            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(PhoneNumber))
            {
                query = query.Where(A => A.CustomerName.Contains(Name) && A.CustomerPhoneNumber.Contains(PhoneNumber));
            }
            else if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(t => t.CustomerName.Contains(Name));
            }
            else if (!string.IsNullOrEmpty(PhoneNumber))
            {
                query = query.Where(t => t.CustomerPhoneNumber.Contains(PhoneNumber));
            }
            var result = await query.ToListAsync();
            return _mapper.Map<IReadOnlyList<Customer>, IReadOnlyList<ReadCustomerDto>>(result);
        }
    }
}
