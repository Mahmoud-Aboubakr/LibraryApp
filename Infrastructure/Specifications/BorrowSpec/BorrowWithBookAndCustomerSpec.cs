using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.BorrowSpec
{
    public class BorrowWithBookAndCustomerSpec : EntitySpec<Borrow>
    {
        public BorrowWithBookAndCustomerSpec()
        {
            AddInclude(B => B.Book);
            AddInclude(B => B.Customer);
        }
        public BorrowWithBookAndCustomerSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(B => B.Book);
            AddInclude(B => B.Customer);
        }
    }
}
