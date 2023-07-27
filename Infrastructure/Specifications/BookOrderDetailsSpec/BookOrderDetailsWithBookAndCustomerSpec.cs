using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.BookOrderDetailsSpec
{
    public class BookOrderDetailsWithBookAndCustomerSpec : BaseSpecification<BookOrderDetails>
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

        public BookOrderDetailsWithBookAndCustomerSpec(int pageSize = 6, int pageIndex = 1, bool isPagingEnabled = true)
        {
            AddInclude(B => B.Book);
            AddInclude(B => B.Order.Customer);
            AddOrederBy(B => B.OrderId);
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "Asc":
                        AddOrederBy(B => B.OrderId);
                        break;
                    case "Desc":
                        AddOrederByDescending(B => B.OrderId);
                        break;
                    default:
                        AddOrederBy(B => B.OrderId);
                        break;
                }
            }
        }

        public BookOrderDetailsWithBookAndCustomerSpec(int? id = null, int? bookId = null) : base(x => x.BookId == bookId)
        {
        }

        public BookOrderDetailsWithBookAndCustomerSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(B => B.Book);
            AddInclude(B => B.Order.Customer);
        }
    }
}
