using Application.DTOs;
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
    public class VacationServices : IVacationServices
    {
        private readonly LibraryDbContext _context;
        public VacationServices(LibraryDbContext context)
        {
            _context = context;
        }

        public bool IsValidNormalVacation(bool? normalvacation)
        {
            var result = _context.Vacations.Where(v => v.NormalVacation == true).GroupBy(v => new { v.EmpId, v.DayDate.Month }).Count();
            if (normalvacation == true && result >= 25)
            {
                return false;
            }
            return true;
        }

        public bool IsValidUrgentVacation(bool? urgentvacation)
        {
            var result = _context.Vacations.Where(v => v.UrgentVacation == true).GroupBy(v => new { v.EmpId, v.DayDate.Month }).Count();
            if (urgentvacation == true && result >= 5)
            {
                return false;
            }
            return true;
        }

        public async Task<IReadOnlyList<ReadVacationDetailsDto>> SearchVactionDataWithDetail(string empName = null)
        {
            var result = await (from v in _context.Vacations
                                join e in _context.Employees
                                on v.EmpId equals e.Id
                                where empName == null || e.EmpName.Contains(empName)
                                select new ReadVacationDetailsDto()
                                {
                                    Id = v.Id,
                                    DayDate = v.DayDate,
                                    NormalVacation = v.NormalVacation,
                                    UrgentVacation = v.UrgentVacation,
                                    Absence = v.Absence,
                                    EmpId = e.Id,
                                    EmpName = e.EmpName
                                }).ToListAsync();
            return result;
        }

        public async Task<GetVacationsCountDto> GetTotalVacationsByEmpId(int empId, DateTime FromDate, DateTime ToDate)
        {
            var _NormalVacationCount = await _context.Vacations.Where(v => v.EmpId == empId && v.DayDate >= FromDate && v.DayDate <= ToDate && v.NormalVacation == true)
                                                        .GroupBy(v => new { v.EmpId, v.DayDate }).CountAsync();

            var _UrgentVacationCount = await _context.Vacations.Where(v => v.EmpId == empId && v.DayDate >= FromDate && v.DayDate <= ToDate && v.UrgentVacation == true)
                                                        .GroupBy(v => new { v.EmpId, v.DayDate }).CountAsync();

            var _AbsenceCount = await _context.Vacations.Where(v => v.EmpId == empId && v.DayDate >= FromDate && v.DayDate <= ToDate && v.Absence == true)
                                                        .GroupBy(v => new { v.EmpId, v.DayDate }).CountAsync();

            var result = new GetVacationsCountDto
            {
                EmpId = empId,
                NormalVacationCount = _NormalVacationCount,
                UrgentVacationCount = _UrgentVacationCount,
                AbsenceCount = _AbsenceCount
            };

            return result;
        }
    }
}
