namespace Application.DTOs.Payroll
{
    public class ReadPayrollDto
    {
        public int Id { get; set; }
        public DateTime? SalaryDate { get; set; } = null;
        public decimal BasicSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deduct { get; set; }
        public decimal TotalSalary { get; set; }

        public int EmpId { get; set; }

        public DateTime? CreatedDate { get; set; } 
    }
}
