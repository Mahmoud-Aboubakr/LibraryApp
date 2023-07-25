﻿using Application.DTOs.Payroll;
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
    public class PayrollServices : IPayrollServices
    {
        private readonly LibraryDbContext _context;

        public PayrollServices(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ReadPayrollDto>> SearchPayrollDataWithDetail(string empName = null)
        {
            var result = await(from p in _context.Payrolls
                               join e in _context.Employees
                               on p.EmpId equals e.Id
                               where empName == null || e.EmpName.Contains(empName)
                               select new ReadPayrollDto()
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