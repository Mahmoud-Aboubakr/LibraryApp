﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payroll : BaseEntity
    {
        public int EmpId { get; set; }
        public DateTime? SalaryDate { get; set; } = null;
        public float BasicSalary { get; set; }
        public float Bonus { get; set; }
        public float Deduct { get; set; }
        public float TotalSalary { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
