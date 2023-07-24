using Application.Handlers;
using System.Text.Json.Serialization;

namespace Application.DTOs.Author
{
    public class UpdateAuthorDto
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorPhoneNumber { get; set; }
        public decimal? AuthorProfits { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime CreatedDate { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
