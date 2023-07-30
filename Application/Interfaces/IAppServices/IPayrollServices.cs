using Application.DTOs.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IAppServices
{
    public interface IPayrollServices
    {
        Task<IReadOnlyList<ReadPayrollDto>> SearchPayrollDataWithDetail(string empName = null);
        decimal CalculateDeduct(int absenceDays, decimal dailyPay, int lateHours, decimal hourlyPay);
        decimal CalculateBonus(int extraHours, decimal hourlyPay);
    }
}
