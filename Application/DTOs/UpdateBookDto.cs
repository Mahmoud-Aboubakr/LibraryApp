

namespace Application.DTOs
{
    public class UpdateBookDto
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
    }
}
