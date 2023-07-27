using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.ReturnOrderSpec
{
    public class ReturnedOrderWithCustomerSpec : BaseSpecification<ReturnedOrder>
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
        public ReturnedOrderWithCustomerSpec(int pageSize = 6, int pageIndex = 1, bool isPagingEnabled = true)
        {
            AddInclude(R => R.Customer);

            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);
        }

        public ReturnedOrderWithCustomerSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(R => R.Customer);
        }
    }
}
