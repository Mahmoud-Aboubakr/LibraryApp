using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateAuthorDto
    {
        public string AuthorName { get; set; }
        [RegularExpression(@"^[0-9]+$")]
        public string AuthorPhoneNumber { get; set; }
        public decimal? AuthorProfits { get; set; }
    }
}
