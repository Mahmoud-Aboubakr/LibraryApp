

namespace Application.DTOs
{
    public class BorrowDto
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
