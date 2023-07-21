using Application.DTOs.Author;

namespace Application.Interfaces.IAppServices
{
    public interface IAuthorServices
    {
        Task<IReadOnlyList<ReadAuthorDto>> SearchWithCriteria(string name = null, string phone = null);
    }
}
