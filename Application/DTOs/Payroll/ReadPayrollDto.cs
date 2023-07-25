using Application.Handlers;
using System.Text.Json.Serialization;

namespace Application.DTOs.Payroll
{
    public class ReadPayrollDto
    {
        public int Id { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime SalaryDate { get; set; } 
        public decimal BasicSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deduct { get; set; }
        public decimal TotalSalary { get; set; }

        public int EmpId { get; set; }
        public string EmpName { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; }
    }
}
