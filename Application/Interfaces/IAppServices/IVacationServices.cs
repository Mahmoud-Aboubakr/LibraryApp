using Application.DTOs.Vacation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IAppServices
{
    public interface IVacationServices
    {
        bool IsValidNormalVacation(bool? normalvacation);
        bool IsValidUrgentVacation(bool? urgentvacation);
        Task<IReadOnlyList<ReadVacationDto>> SearchVactionDataWithDetail(string empName = null);
        Task<GetVacationsCountDto> GetTotalVacationsByEmpId(int empId, DateTime FromDate, DateTime ToDate);
        Task<int> GetAbsenceDaysByMonth(int employeeId, int month);
    }
}
