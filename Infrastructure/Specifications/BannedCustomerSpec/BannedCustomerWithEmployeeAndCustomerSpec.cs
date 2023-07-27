using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.BannedCustomerSpec
{
    public class BannedCustomerWithEmployeeAndCustomerSpec : BaseSpecification<BannedCustomer>
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

        public BannedCustomerWithEmployeeAndCustomerSpec(int pageSize = 6, int pageIndex = 1, bool isPagingEnabled = true)
        {
            AddInclude(B => B.Employee);
            AddInclude(B => B.Customer);
            AddOrederBy(B => B.BanDate);
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "Asc":
                        AddOrederBy(B => B.BanDate);
                        break;
                    case "Desc":
                        AddOrederByDescending(B => B.BanDate);
                        break;
                    default:
                        AddOrederBy(B => B.BanDate);
                        break;
                }
            }
        }

        public BannedCustomerWithEmployeeAndCustomerSpec(int? id = null, int? customerId = null) : base(x => x.CustomerId == customerId)
        {
        }

        public BannedCustomerWithEmployeeAndCustomerSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(B => B.Employee);
            AddInclude(B => B.Customer);
        }
    }
}
