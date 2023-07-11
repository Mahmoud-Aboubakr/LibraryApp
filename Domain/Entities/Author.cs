using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Author : BaseEntity
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorPhoneNumber { get; set; }
        public decimal AuthorProfits { get; set; }
    }
}
