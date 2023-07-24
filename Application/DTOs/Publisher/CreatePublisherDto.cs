using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Publisher
{
    public class CreatePublisherDto
    {
        public string PublisherName { get; set; }
        public string PublisherPhoneNumber { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
