using NPoco.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
