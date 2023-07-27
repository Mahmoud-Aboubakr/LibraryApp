using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.EmployeeSpec
{
    public class EmployeeSpec : BaseSpecification<Employee>
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

        public EmployeeSpec(int id) : base(x => x.Id == id)
        {
        }
    
        public EmployeeSpec(int pageSize = 6, int pageIndex = 1, bool isPagingEnabled = true)
           : base()
        {
            AddOrederBy(a => a.EmpName);
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "Asc":
                        AddOrederBy(a => a.EmpName);
                        break;
                    case "Desc":
                        AddOrederByDescending(a => a.EmpName);
                        break;
                    default:
                        AddOrederBy(a => a.EmpName);
                        break;
                }
            }
        }
    }
}
