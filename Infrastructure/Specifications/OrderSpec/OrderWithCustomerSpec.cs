using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.OrderSpec
{
    public class OrderWithCustomerSpec : EntitySpec<Order>
    {
        public OrderWithCustomerSpec()
        {
            AddInclude(O => O.Customer);
        }
        public OrderWithCustomerSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(O => O.Customer);
        }
    }
}
