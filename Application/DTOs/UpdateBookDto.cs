

namespace Application.DTOs
{
    public class UpdateBookDto
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
    }
}
