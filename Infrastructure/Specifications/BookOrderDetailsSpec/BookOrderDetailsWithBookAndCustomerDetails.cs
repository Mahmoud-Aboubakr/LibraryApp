using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.BookOrderDetailsSpec
{
    public class BookOrderDetailsWithBookAndCustomerDetails : EntitySpec<BookOrderDetails>
    {
        public BookOrderDetailsWithBookAndCustomerDetails()
        {
            AddInclude(B => B.Book);
            AddInclude(B => B.Order.Customer);
        }
        public BookOrderDetailsWithBookAndCustomerDetails(int id) : base(x => x.Id == id)
        {
            AddInclude(B => B.Book);
            AddInclude(B => B.Order.Customer);
        }
    }
}
