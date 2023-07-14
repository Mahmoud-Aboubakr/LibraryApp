

namespace Domain.Entities
{
    public class BannedCustomer : BaseEntity
    {
        public int CustomerId { get; set; }
        public DateTime? BanDate { get; set; } = null;
        public int EmpId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
