using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AppServices
{
    public class SearchPayrollDataWithDetailService : ISearchPayrollDataWithDetailService
    {
        private readonly LibraryDbContext _context;

        public SearchPayrollDataWithDetailService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ReadPayrollDetailsDto>> SearchPayrollDataWithDetail(string empName = null)
        {
            var result = await(from p in _context.Payrolls
                               join e in _context.Employees
                               on p.EmpId equals e.Id
                               where empName == null || e.EmpName.Contains(empName)
                               select new ReadPayrollDetailsDto()
                               {
                                   Id = p.Id,
                                   SalaryDate = p.SalaryDate,
                                   BasicSalary = p.BasicSalary,
                                   Bonus = p.Bonus,
                                   Deduct = p.Deduct,
                                   TotalSalary = p.TotalSalary,
                                   EmpId = e.Id,
                                   EmpName = e.EmpName
                               }).ToListAsync();
            return result;
        }
    }
}
