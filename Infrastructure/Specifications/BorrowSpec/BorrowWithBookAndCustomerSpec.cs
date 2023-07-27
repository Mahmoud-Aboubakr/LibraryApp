using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.BorrowSpec
{
    public class BorrowWithBookAndCustomerSpec : BaseSpecification<Borrow>
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

        public BorrowWithBookAndCustomerSpec(int pageSize = 6, int pageIndex = 1, bool isPagingEnabled = true)
        {
            AddInclude(B => B.Book);
            AddInclude(B => B.Customer);
            AddOrederBy(B => B.BorrowDate);
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "Asc":
                        AddOrederBy(B => B.BorrowDate);
                        break;
                    case "Desc":
                        AddOrederByDescending(B => B.BorrowDate);
                        break;
                    default:
                        AddOrederBy(B => B.BorrowDate);
                        break;
                }
            }
        }

        public BorrowWithBookAndCustomerSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(B => B.Book);
            AddInclude(B => B.Customer);
        }
    }
}
