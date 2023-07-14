

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Ordertype { get; set; }
        //public virtual ICollection<Book> Books { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
