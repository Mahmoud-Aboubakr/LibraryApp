using Application.DTOs.Employee;
using Application.Interfaces.IAppServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Infrastructure.AppServices
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeServices(LibraryDbContext context, IMapper mapper)
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
        public bool IsValidEmployeeAge(int age)
        {
            return age >= 20 && age <= 60;
        }

        public bool IsValidEmployeeType(byte EmpType)
        {
            int[] validTypes = { 0, 1, 2, 3 };
            return validTypes.Contains(EmpType);
        }

        public async Task<decimal> CalculateHourlyPay(int employeeId)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            var startShift = employee.EmpStartingShift;
            var endShift = employee.EmpEndingShift;
            var workingHours = (int)(endShift - startShift).TotalHours;
            var workingDays = 5 * DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) / 7;
            var hourlyPay = employee.EmpBasicSalary / (workingDays * workingHours);

            return hourlyPay;
        }

        public async Task<decimal> CalculateDailyPay(int employeeId)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            var startShift = employee.EmpStartingShift;
            var endShift = employee.EmpEndingShift;
            var workingHours = (int)(endShift - startShift).TotalHours;
            var workingDays = 5 * DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) / 7;
            var dailyPay = employee.EmpBasicSalary / workingDays;

            return dailyPay;
        }
    }
}
