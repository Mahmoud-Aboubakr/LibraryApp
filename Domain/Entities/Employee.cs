using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee : BaseEntity
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public byte EmpType { get; set; }
        public int EmpAge { get; set; }
        public string EmpAddress { get; set; }
        public string EmpPhoneNumber { get; set; }
        public TimeSpan EmpStartingShift { get; set; }
        public TimeSpan EmpEndingShift { get; set; }
        public decimal EmpBasicSalary { get; set; }

        public Attendence Attendence { get; set; }
        public Payroll Payroll { get; set; }
        public Vacation Vacation { get; set; }
    }
}
