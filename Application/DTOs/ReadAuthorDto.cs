

namespace Domain.Entities
{
    public class ReadAuthorDto
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorPhoneNumber { get; set; }
        public decimal? AuthorProfits { get; set; }
    }
}
