using Application.DTOs;

namespace Application.Interfaces.IAppServices
{
    public interface IBookServices
    {
        Task<IReadOnlyList<ReadBookDto>> SearchBookDataWithDetail(string bookTitle = null, string authorName = null, string publisherName = null);
    }
}
