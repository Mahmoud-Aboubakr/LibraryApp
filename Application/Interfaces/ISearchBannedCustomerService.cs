﻿using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISearchBannedCustomerService
    {
        Task<IReadOnlyList<ReadBannedCustomerDto>> SearchForBannedCustomer(string EmpName = null , string CustomerName = null);
    }
}