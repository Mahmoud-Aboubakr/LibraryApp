using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.BookSpec
{
    public class BooksWithAuthorAndPublisherSpec : BaseSpecification<Book>
    {
        public string Sort { get; set; }
        private int MaxPageSize { get; set; }
        private int _pageSize { get; set; }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public int PageIndex { get; set; }

        public string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }

        public BooksWithAuthorAndPublisherSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(b => b.Author);
            AddInclude(b => b.Publisher);
        }

        public BooksWithAuthorAndPublisherSpec(int pageSize = 6, int maxSize = 50,int pageIndex =1)
           : base()
        {

            AddInclude(b => b.Author);
            AddInclude(b => b.Publisher);
            AddOrederBy(b => b.BookTitle);
            ApplyPanging(pageSize * (pageIndex - 1), pageSize);

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "Asc":
                        AddOrederBy(b => b.BookTitle);
                        break;
                    case "Desc":
                        AddOrederByDescending(b => b.BookTitle);
                        break;
                    default:
                        AddOrederBy(b => b.BookTitle);
                        break;
                }
            }
        }
    }
}
