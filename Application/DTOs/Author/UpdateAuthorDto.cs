namespace Application.DTOs.Author
{
    public class UpdateAuthorDto
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorPhoneNumber { get; set; }
        public decimal? AuthorProfits { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
