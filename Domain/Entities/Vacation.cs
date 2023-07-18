

namespace Domain.Entities
{
    public class Vacation : BaseEntity
    {
        public int EmpId { get; set; }
        public DateTime DayDate { get; set; }
        public bool? NormalVacation { get; set; }
        public bool? UrgentVacation { get; set; }
        public bool? Absence { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
