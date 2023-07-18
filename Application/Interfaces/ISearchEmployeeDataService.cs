using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISearchEmployeeDataService
    {
        Task<IReadOnlyList<ReadEmployeeDto>> SearchEmployeeDataWithDetail(string? empName = null, byte? empType = null, string? empPhoneNumber = null, decimal? empBasicSalary = null);
    }
}
