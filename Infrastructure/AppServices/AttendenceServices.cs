﻿using Application.DTOs.Attendance;
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

        public async Task<IReadOnlyList<ReadAttendanceDto>> SearchAttendenceDataWithDetail(string empName = null)
        {
            _context.Vacations.Where(v => v.NormalVacation == true).GroupBy(v => v.EmpId).Count();
            var result = await (from a in _context.Attendence
                                join e in _context.Employees
                                on a.EmpId equals e.Id
                                where empName == null || e.EmpName.Contains(empName)
                                select new ReadAttendanceDto()
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
            return permission == 0 || permission == 1 || permission == 2;
        }

        public bool IsValidPermission(int permission)
        {
            var result = _context.Attendence.Where(A => A.Permission == 1).GroupBy(A => new { A.EmpId, A.Month }).Count();
            if (permission == 1 && result >= 2)
            {
                return false;
            }
            return true;
        }

        public async Task<int> GetLateHoursByMonth(int employeeId, int month)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee == null)
            {
                throw new ArgumentException($"Employee with ID {employeeId} not found.");
            }

            var startShift = employee.EmpStartingShift.TimeOfDay;
            var endShift = employee.EmpEndingShift.TimeOfDay;

            var attendances = await _context.Attendence.Where(a => a.EmpId == employeeId && a.DayDate.Month == month && a.Permission == 2).ToListAsync();

            var totalLateHours = 0;

            foreach (var attendance in attendances)
            {
                var arrivalTime = attendance.EmpArrivalTime.TimeOfDay;
                var leavingTime = attendance.EmpLeavingTime.TimeOfDay;

                var HoursDiff = (arrivalTime - startShift) + (endShift - leavingTime);
                int lateHours = (int)HoursDiff.TotalHours;
                if (lateHours < 0)
                {
                    lateHours = -lateHours;
                }

                totalLateHours += lateHours;
            }

            return totalLateHours;
        }

        public async Task<int> GetExtraHoursByMonth(int employeeId, int month)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee == null)
            {
                throw new ArgumentException($"Employee with ID {employeeId} not found.");
            }

            var startShift = employee.EmpStartingShift.TimeOfDay;
            var endShift = employee.EmpEndingShift.TimeOfDay;

            var attendances = await _context.Attendence.Where(a => a.EmpId == employeeId && a.DayDate.Month == month).ToListAsync();

            var totalExtraHours = 0;

            foreach (var attendance in attendances)
            {
                var arrivalTime = attendance.EmpArrivalTime.TimeOfDay;
                var leavingTime = attendance.EmpLeavingTime.TimeOfDay;

                var HoursDiff = leavingTime - endShift;
                int extraHours = (int)HoursDiff.TotalHours;
                if (extraHours < 0)
                {
                    extraHours = 0;
                }

                totalExtraHours += extraHours;
            }

            return totalExtraHours;
        }
    }
}
