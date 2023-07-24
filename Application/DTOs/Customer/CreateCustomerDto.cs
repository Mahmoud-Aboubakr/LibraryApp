using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Customer
{
    public class CreateCustomerDto
    {
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
