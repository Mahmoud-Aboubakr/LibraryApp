using Application.DTOs.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IAppServices
{
    public interface IEmployeeServices
    {
        bool IsValidEmployeeAge(int age);
        bool IsValidEmployeeType(byte type);
        Task<IReadOnlyList<ReadEmployeeDto>> SearchEmployeeDataWithDetail(string empName = null, byte? empType = null, string empPhoneNumber = null, decimal? empBasicSalary = null);
        Task<decimal> CalculateHourlyPay(int employeeId);
        Task<decimal> CalculateDailyPay(int employeeId);
    }
}
