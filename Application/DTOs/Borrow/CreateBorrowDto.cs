namespace Application.DTOs.Borrow
{
    public class CreateBorrowDto
    {
        public string CustomerId { get; set; }
        public string BookId { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

    }
}
