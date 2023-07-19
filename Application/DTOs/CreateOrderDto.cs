using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateOrderDto
    {
        public string CustomerId { get; set; }
      
        public string TotalPrice { get; set; }
    }
}
