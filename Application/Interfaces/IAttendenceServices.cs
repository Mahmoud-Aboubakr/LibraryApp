using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAttendenceServices
    {
        bool IsValidAttendencePermission(int permission);
        bool IsValidMonth(byte month);
        Task<IReadOnlyList<ReadAttendenceDetailsDto>> SearchAttendenceDataWithDetail(string empName = null);
    }
}
