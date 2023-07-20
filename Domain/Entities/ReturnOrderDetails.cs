using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ReturnOrderDetails : BaseEntity
    {
        public int ReturnedOrderId { get; set; }
        public int BookId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ReturnedOrder Order { get; set; }
        public Book Book { get; set; }
    }
}
