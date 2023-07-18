using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AppServices
{
    public class SearchAttendenceDataWithDetailService : ISearchAttendenceDataWithDetailService
    {
        private readonly LibraryDbContext _context;

        public SearchAttendenceDataWithDetailService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ReadAttendenceDetailsDto>> SearchAttendenceDataWithDetail(string empName = null)
        {
            _context.Vacations.Where(v => v.NormalVacation == true).GroupBy(v => v.EmpId).Count();
            var result = await (from a in _context.Attendence
                                join e in _context.Employees
                                on a.EmpId equals e.Id
                                where empName == null || e.EmpName.Contains(empName)
                                select new ReadAttendenceDetailsDto()
                                {
                                    Id = a.Id,
                                    EmpArrivalTime = a.EmpArrivalTime,
                                    EmpLeavingTime = a.EmpLeavingTime,
                                    Permission = a.Permission,
                                    DayDate = a.DayDate,
                                    Month = a.Month,
                                    EmpId = e.Id,
                                    EmpName = e.EmpName
                                }).ToListAsync();
            return result;
        }
    }
}
