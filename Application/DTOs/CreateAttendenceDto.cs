using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateAttendenceDto
    {
        public DateTime? EmpArrivalTime { get; set; } = null;
        public DateTime? EmpLeavingTime { get; set; } = null;
        public int Permission { get; set; }
        public DateTime? DayDate { get; set; } = null;
        public byte Month { get; set; }
        public int EmpId { get; set; }
    }
}
