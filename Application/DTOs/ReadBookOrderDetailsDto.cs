

namespace Application.DTOs
{
    public class ReadBookOrderDetailsDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
