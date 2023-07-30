

using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class Attendence : BaseEntity
    {
        public int EmpId { get; set; }
        public DateTime EmpArrivalTime { get; set; }
        public DateTime EmpLeavingTime { get; set; }
        public int Permission { get; set; }
        public DateTime DayDate { get; set; }
        public int Month { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
