using Application.DTOs.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IAppServices
{
    public interface IAttendenceServices
    {
        bool IsValidAttendencePermission(int permission);
        Task<IReadOnlyList<ReadAttendanceDto>> SearchAttendenceDataWithDetail(string empName = null);
        bool IsValidPermission(int permission);
        Task<int> GetLateHoursByMonth(int employeeId, int month);
        Task<int> GetExtraHoursByMonth(int employeeId, int month);
    }
}
