using Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EmpStartingShift { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EmpEndingShift { get; set; }
        public decimal EmpBasicSalary { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
