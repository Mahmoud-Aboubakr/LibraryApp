using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.VacationSpec
{
    public class VacationWithEmployeeSpec : BaseSpecification<Vacation>
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
        public VacationWithEmployeeSpec(int pageSize = 6, int pageIndex = 1, bool isPagingEnabled = true)
        {
            AddInclude(v => v.Employee);

            AddOrederBy(p => p.Employee.EmpName);
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "Asc":
                        AddOrederBy(p => p.Employee.EmpName);
                        break;
                    case "Desc":
                        AddOrederByDescending(p => p.Employee.EmpName);
                        break;
                    default:
                        AddOrederBy(p => p.Employee.EmpName);
                        break;
                }
            }
        }

        public VacationWithEmployeeSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(v => v.Employee);
        }

        public VacationWithEmployeeSpec(int? id = null, int? empId = null) : base(x => x.EmpId == empId)
        {
        }
    }
}
