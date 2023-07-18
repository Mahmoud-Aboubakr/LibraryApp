

namespace Domain.Entities
{
    public class BookOrderDetails : BaseEntity
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; }
        public Book Book { get; set; }
    }
}
