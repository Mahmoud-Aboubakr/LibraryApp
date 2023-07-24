namespace Application.DTOs.Publisher
{
    public class ReadPublisherDto
    {
        public int Id { get; set; }
        public string PublisherName { get; set; }
        public string PublisherPhoneNumber { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
