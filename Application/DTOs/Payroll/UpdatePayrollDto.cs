using Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.Payroll
{
    public class UpdatePayrollDto
    {
        public int Id { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime SalaryDate { get; set; } 
        public decimal Bonus { get; set; }
        public decimal Deduct { get; set; }
        public decimal TotalSalary { get; set; }

        public int EmpId { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
