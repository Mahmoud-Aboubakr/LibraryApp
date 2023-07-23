using Application.DTOs.Attendance;
using Application.Interfaces.IAppServices;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Infrastructure.AppServices
{
    public class AttendenceServices : IAttendenceServices
    {
        private readonly LibraryDbContext _context;

        public AttendenceServices(LibraryDbContext context)
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
        public bool IsValidAttendencePermission(int permission)
        {
            return permission == 0 || permission == 1;
        }
        public bool IsValidMonth(byte month)
        {
            return month >= 1 && month <= 12;
        }
    }
}
