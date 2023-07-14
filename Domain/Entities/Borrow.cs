

namespace Domain.Entities
{
    public class Borrow : BaseEntity
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public virtual Order Order { get; set; }
        public virtual Book Book { get; set; }
    }
}
