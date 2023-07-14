

namespace Application.DTOs
{
    public class PayrollDto
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime? SalaryDate { get; set; } = null;
        public float BasicSalary { get; set; }
        public float Bonus { get; set; }
        public float Deduct { get; set; }
        public float TotalSalary { get; set; }
    }
}
