

namespace Domain.Entities
{
    public class OrderBooks : BaseEntity
    {
        public Order Orders {  get; set; }
        public int OrderId { get; set; }
        public Book Books { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
