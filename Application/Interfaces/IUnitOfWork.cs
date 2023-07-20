

using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUnitOfWork<T> : IDisposable where T : BaseEntity
    {
        IGenericRepository<T> GetRepository();
        Task<int> Commit();
    }
}
