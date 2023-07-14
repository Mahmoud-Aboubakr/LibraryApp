using Application.DTOs;

namespace Infrastructure.AppServicesContracts
{
    public interface ISearchBookDataWithDetailService
    {
        Task<IReadOnlyList<ReadBookDto>> SearchBookDataWithDetail(string bookTitle = null, string authorName = null, string publisherName = null);
    }
}
