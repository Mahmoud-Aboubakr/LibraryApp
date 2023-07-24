namespace Application.DTOs.Attendance
{
    public class ReadAttendanceDto
    {
        public int Id { get; set; }
        public DateTime? EmpArrivalTime { get; set; } = null;
        public DateTime? EmpLeavingTime { get; set; } = null;
        public int Permission { get; set; }
        public DateTime? DayDate { get; set; } = null;
        public byte Month { get; set; }

        public int EmpId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
