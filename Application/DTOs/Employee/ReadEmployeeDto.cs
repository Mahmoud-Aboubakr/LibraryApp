using Application.Handlers;
using System.Text.Json.Serialization;

namespace Application.DTOs.Employee
{
    public class ReadEmployeeDto
    {
        public int Id { get; set; }
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
        public DateTime CreatedDate { get; set; }
    }
}
