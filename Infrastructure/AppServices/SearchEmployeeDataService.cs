using Application.DTOs;
using Application.Interfaces;
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
    public class SearchEmployeeDataService : ISearchEmployeeDataService
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public SearchEmployeeDataService(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ReadEmployeeDto>> SearchEmployeeDataWithDetail(string empName = null, byte? empType = null, string empPhoneNumber = null, decimal? empBasicSalary = null)
        {
            var query = _context.Set<Employee>().AsQueryable();

            if (!string.IsNullOrEmpty(empName))
            {
                query = query.Where(e => e.EmpName.Contains(empName));
            }

            if (empType.HasValue)
            {
                query = query.Where(e => e.EmpType == empType.Value);
            }

            if (!string.IsNullOrEmpty(empPhoneNumber))
            {
                query = query.Where(e => e.EmpPhoneNumber.Contains(empPhoneNumber));
            }

            if (empBasicSalary.HasValue)
            {
                query = query.Where(e => e.EmpBasicSalary == empBasicSalary.Value);
            }

            var result = await query.ToListAsync();

            return _mapper.Map<IReadOnlyList<Employee>, IReadOnlyList<ReadEmployeeDto>>(result);
        }
    }
}
