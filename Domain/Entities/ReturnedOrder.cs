using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ReturnedOrder : BaseEntity
    {
        public int OriginOrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        //public virtual Order Order { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
