namespace Application.DTOs.Book
{
    public class CreateBookDto
    {
        public string BookTitle { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
