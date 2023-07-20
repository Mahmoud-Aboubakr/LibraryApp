using Domain.Entities;


namespace Application.Interfaces.IAppServices
{
    public interface IAuthorServices
    {
        Task<IReadOnlyList<ReadAuthorDto>> SearchWithCriteria(string name = null, string phone = null);
    }
}
