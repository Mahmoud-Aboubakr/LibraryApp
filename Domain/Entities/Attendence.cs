

using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class Attendence : BaseEntity
    {
        public int EmpId { get; set; }
        [AllowNull]
        //[Column(TypeName = "time(7)")]
        public TimeOnly EmpArrivalTime { get; set; } 
        [AllowNull]
        //[Column(TypeName = "time(7)")]
        public TimeOnly EmpLeavingTime { get; set; } 
        public int Permission { get; set; }
        public DateTime? DayDate { get; set; } = null;
        public byte Month { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
