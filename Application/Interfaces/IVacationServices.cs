﻿using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IVacationServices
    {
        bool IsValidNormalVacation(bool? normalvacation);
        bool IsValidUrgentVacation(bool? urgentvacation);
        Task<IReadOnlyList<ReadVacationDetailsDto>> SearchVactionDataWithDetail(string empName = null);
        Task<GetVacationsCountDto> GetTotalVacationsByEmpId(int empId, DateTime FromDate, DateTime ToDate);
    }
}
