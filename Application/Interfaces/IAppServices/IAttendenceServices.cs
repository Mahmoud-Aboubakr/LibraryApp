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
        bool IsValidMonth(byte month);
        //Task<IReadOnlyList<ReadAttendanceDto>> SearchAttendenceDataWithDetail(string empName = null);
    }
}
