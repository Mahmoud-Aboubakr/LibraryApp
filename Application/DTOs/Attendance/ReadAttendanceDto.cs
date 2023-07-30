using Application.Handlers;
using System.Text.Json.Serialization;

namespace Application.DTOs.Attendance
{
    public class ReadAttendanceDto
    {
        public int Id { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? EmpArrivalTime { get; set; } 
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? EmpLeavingTime { get; set; } 
        public int Permission { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? DayDate { get; set; }
        public int Month { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; }
    }
}
