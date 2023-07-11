using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Attendence : BaseEntity
    {
        public int EmployeeId { get; set; }
        public TimeSpan EmpArrivalTime { get; set; }
        public TimeSpan EmpLeavingTime { get; set; }
        public int Permission { get; set; }
        public DateTime DayDate { get; set; }
        public byte Month { get; set; }

        public Employee Employee { get; set; }
    }
}
