using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Employee
{
    public class CreateEmployeeDto
    {
        public string EmpName { get; set; }
        public byte EmpType { get; set; }
        public int EmpAge { get; set; }
        public string EmpAddress { get; set; }
        public string EmpPhoneNumber { get; set; }
        public DateTime EmpStartingShift { get; set; }
        public DateTime EmpEndingShift { get; set; }
        public decimal EmpBasicSalary { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
