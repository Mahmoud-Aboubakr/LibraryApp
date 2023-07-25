namespace Application.DTOs.Attendance
{
    public class ReadAttendanceDto
    {
        public int Id { get; set; }
        public TimeOnly? EmpArrivalTime { get; set; } = null;
        public TimeOnly? EmpLeavingTime { get; set; } = null;
        public int Permission { get; set; }
        public DateTime? DayDate { get; set; } = null;
        public byte Month { get; set; }

        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
