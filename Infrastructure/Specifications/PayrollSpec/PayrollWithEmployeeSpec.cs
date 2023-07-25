﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.PayrollSpec
{
    public class PayrollWithEmployeeSpec : BaseSpecification<Payroll>
    {
        public PayrollWithEmployeeSpec()
        {
            AddInclude(p => p.Employee);
        }

        public PayrollWithEmployeeSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(p => p.Employee);
        }
    }
}
