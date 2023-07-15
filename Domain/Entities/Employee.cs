

namespace Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string EmpName { get; set; }
        public byte EmpType { get; set; }
        public int EmpAge { get; set; }
        public string EmpAddress { get; set; }
        public string EmpPhoneNumber { get; set; }
        public DateTime? EmpStartingShift { get; set; } = null;
        public DateTime? EmpEndingShift { get; set; } = null;
        public decimal EmpBasicSalary { get; set; }

        public virtual Attendence Attendence { get; set; }
        public virtual Payroll Payroll { get; set; }
        public virtual Vacation Vacation { get; set; }
    }
}
