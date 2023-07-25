

namespace Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string EmpName { get; set; }
        public byte EmpType { get; set; }
        public int EmpAge { get; set; }
        public string EmpAddress { get; set; }
        public string EmpPhoneNumber { get; set; }
        public DateTime EmpStartingShift { get; set; }
        public DateTime EmpEndingShift { get; set; }
        public decimal EmpBasicSalary { get; set; }
    }
}
