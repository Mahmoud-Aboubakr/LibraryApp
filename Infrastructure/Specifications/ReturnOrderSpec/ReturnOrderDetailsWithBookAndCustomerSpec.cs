using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.ReturnOrderSpec
{
    public class ReturnOrderDetailsWithBookAndCustomerSpec : BaseSpecification<ReturnOrderDetails>
    {
        public ReturnOrderDetailsWithBookAndCustomerSpec()
        {
            AddInclude(R => R.Order.Customer);
            AddInclude(R => R.Book);
        }

        public ReturnOrderDetailsWithBookAndCustomerSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(R => R.Order.Customer);
            AddInclude(R => R.Book);
        }
    }
}
