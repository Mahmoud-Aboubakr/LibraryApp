using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.ReturnOrderSpec
{
    public class ReturnedOrderWithCustomerSpec : BaseSpecification<ReturnedOrder>
    {
        public ReturnedOrderWithCustomerSpec()
        {
            AddInclude(R => R.Customer);
        }

        public ReturnedOrderWithCustomerSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(R => R.Customer);
        }
    }
}
