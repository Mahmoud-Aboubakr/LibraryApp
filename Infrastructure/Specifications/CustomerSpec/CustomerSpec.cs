using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.CustomerSpec
{
    public class CustomerSpec : BaseSpecification<Customer>
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

        public CustomerSpec(int id) : base(x => x.Id == id)
        {
        }

        public CustomerSpec(int pageSize = 6, int pageIndex = 1, bool isPagingEnabled = true)
           : base()
        {
            AddOrederBy(c => c.CustomerName);
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "Asc":
                        AddOrederBy(c => c.CustomerName);
                        break;
                    case "Desc":
                        AddOrederByDescending(c => c.CustomerName);
                        break;
                    default:
                        AddOrederBy(c => c.CustomerName);
                        break;
                }
            }
        }
    }
}
