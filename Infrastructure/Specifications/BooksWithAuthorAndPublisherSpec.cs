using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class BooksWithAuthorAndPublisherSpec : EntitySpec<Book>
    {
        public BooksWithAuthorAndPublisherSpec()
        {
            AddInclude(b => b.Author);
            AddInclude(b => b.Publisher);
        }
        public BooksWithAuthorAndPublisherSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(b => b.Author);
            AddInclude(b => b.Publisher);
        }
    }
}
