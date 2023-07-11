using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Publisher : BaseEntity
    {
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
        public string PublisherPhoneNumber { get; set;}
    }
}
