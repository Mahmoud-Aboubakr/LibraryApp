using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.PublisherSpec
{
    public class PublisherSpec : BaseSpecification<Publisher>
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

        public PublisherSpec(int id) : base(x => x.Id == id)
        {
        }

        public PublisherSpec(int pageSize = 6, int pageIndex = 1, bool isPagingEnabled = true)
        {
            AddOrederBy(p => p.PublisherName);
            ApplyPanging(pageSize * (pageIndex - 1), pageSize, isPagingEnabled);

            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "Asc":
                        AddOrederBy(p => p.PublisherName);
                        break;
                    case "Desc":
                        AddOrederByDescending(p => p.PublisherName);
                        break;
                    default:
                        AddOrederBy(p => p.PublisherName);
                        break;
                }
            }
        }
    }
}
