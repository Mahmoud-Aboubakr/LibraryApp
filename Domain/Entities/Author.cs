

namespace Domain.Entities
{
    public class Author : BaseEntity
    {
        public string AuthorName { get; set; }
        public string AuthorPhoneNumber { get; set; }
        public decimal? AuthorProfits { get; set; }
    }
}
