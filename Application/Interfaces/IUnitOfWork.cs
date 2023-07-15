

using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericBaseRepository<Author> Authors { get; }
        Task<int> SaveChangesAsync();
    }
}
