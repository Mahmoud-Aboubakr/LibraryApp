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
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EmpArrivalTime { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EmpLeavingTime { get; set; }
        public int Permission { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime DayDate { get; set; }
        public byte Month { get; set; }
        public int EmpId { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
