using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
