using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.BannedCustomerSpec
{
    public class BannedCustomerWithEmployeeAndCustomerSpec : EntitySpec<BannedCustomer>
    {
        public BannedCustomerWithEmployeeAndCustomerSpec()
        {
            AddInclude(B => B.Employee);
            AddInclude(B => B.Customer);
        }
        public BannedCustomerWithEmployeeAndCustomerSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(B => B.Employee);
            AddInclude(B => B.Customer);
        }
    }
}
