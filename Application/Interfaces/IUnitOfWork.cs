

using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable 
    {
        IGenericRepository<T> GetRepository<T>() where T : BaseEntity;
        Task<int> Commit();
    }
}
