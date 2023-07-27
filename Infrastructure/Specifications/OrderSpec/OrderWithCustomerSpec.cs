using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.OrderSpec
{
    public class OrderWithCustomerSpec : BaseSpecification<Order>
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
        public OrderWithCustomerSpec(int pageSize = 6, int pageIndex = 1, bool isPagingEnabled = true)
        {
            AddInclude(O => O.Customer);
            AddOrederBy(O => O.OrderDate);
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "Asc":
                        AddOrederBy(O => O.OrderDate);
                        break;
                    case "Desc":
                        AddOrederByDescending(O => O.OrderDate);
                        break;
                    default:
                        AddOrederBy(O => O.OrderDate);
                        break;
                }
            }
        }
        public OrderWithCustomerSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(O => O.Customer);
        }
    }
}
