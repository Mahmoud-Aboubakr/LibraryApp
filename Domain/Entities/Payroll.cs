

namespace Domain.Entities
{
    public class Payroll : BaseEntity
    {
        public int EmpId { get; set; }
        public DateTime SalaryDate { get; set; } 
        public decimal BasicSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deduct { get; set; }
        public decimal TotalSalary { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
