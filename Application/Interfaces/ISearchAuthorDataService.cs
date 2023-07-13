using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISearchAuthorDataService
    {
        Task<IReadOnlyList<ReadAuthorDto>> SearchWithCriteria(string name = null, string phone = null);
    }
}
