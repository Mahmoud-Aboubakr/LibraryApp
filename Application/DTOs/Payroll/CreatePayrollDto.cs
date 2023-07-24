using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payroll
{
    public class CreatePayrollDto
    {
        public DateTime? SalaryDate { get; set; } = null;
        public decimal BasicSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deduct { get; set; }
        public decimal TotalSalary { get; set; }

        public int EmpId { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
