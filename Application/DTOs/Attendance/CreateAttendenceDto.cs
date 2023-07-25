using Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.Attendance
{
    public class CreateAttendenceDto
    {
        [JsonConverter(typeof(CustomTimeConverter))]
        public TimeOnly? EmpArrivalTime { get; set; } = null;
        [JsonConverter(typeof(CustomTimeConverter))]
        public TimeOnly? EmpLeavingTime { get; set; } = null;
        public int Permission { get; set; }
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? DayDate { get; set; } = null;
        public byte Month { get; set; }
        public int EmpId { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
