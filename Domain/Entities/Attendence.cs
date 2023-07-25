

using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class Attendence : BaseEntity
    {
        public int EmpId { get; set; }
        public DateTime? EmpArrivalTime { get; set; } = null;
        public DateTime? EmpLeavingTime { get; set; } = null;
        public int Permission { get; set; }
        public DateTime? DayDate { get; set; } = null;
        public byte Month { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
