using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AppServicesContracts
{
    public interface ISearchBookDataWithDetailService
    {
        Task<IReadOnlyList<ReadBookDto>> SearchBookDataWithDetail(string bookTitle = null, string authorName = null, string publisherName = null);
    }
}
