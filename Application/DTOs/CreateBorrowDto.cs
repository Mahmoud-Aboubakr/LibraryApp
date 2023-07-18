

namespace Application.DTOs
{
    public class CreateBorrowDto
    {
        public string CustomerId { get; set; }
        public string BookId { get; set; }

        //public DateTime BorrowDate { get; set; }
        //public DateTime ReturnDate { get; set; }
    }
}
