using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.BookSpec
{
    public class AuthorSpec : BaseSpecification<Author>
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

        public AuthorSpec(int id) : base(x => x.Id == id)
        {
        }

        public AuthorSpec(int pageSize = 6, int pageIndex = 1, bool isPagingEnabled = true)
           : base()
        {
            AddOrederBy(a => a.AuthorName);
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "Asc":
                        AddOrederBy(a => a.AuthorName);
                        break;
                    case "Desc":
                        AddOrederByDescending(a => a.AuthorName);
                        break;
                    default:
                        AddOrederBy(a => a.AuthorName);
                        break;
                }
            }
        }
    }
}
