

namespace Application.DTOs
{
    public class ReadBookDto
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
    }
}
