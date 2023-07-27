using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.AttendanceSpec
{
    public class AttendanceWithEmployeeSpec : BaseSpecification<Attendence>
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

        public AttendanceWithEmployeeSpec(int pageSize = 6, int pageIndex = 1, bool isPagingEnabled = true)
        {
            AddInclude(A => A.Employee);
            AddOrederBy(A => A.EmpId);
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "Asc":
                        AddOrederBy(A => A.EmpId);
                        break;
                    case "Desc":
                        AddOrederByDescending(A => A.EmpId);
                        break;
                    default:
                        AddOrederBy(A => A.EmpId);
                        break;
                }
            }
        }

        public AttendanceWithEmployeeSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(A => A.Employee);
        }
        public AttendanceWithEmployeeSpec(int? id = null, int? empId = null) : base(x => x.EmpId == empId)
        {
        }
    }
}
