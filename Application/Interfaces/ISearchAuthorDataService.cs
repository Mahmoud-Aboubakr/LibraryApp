using Domain.Entities;


namespace Application.Interfaces
{
    public interface ISearchAuthorDataService
    {
        Task<IReadOnlyList<ReadAuthorDto>> SearchWithCriteria(string name = null, string phone = null);
    }
}
