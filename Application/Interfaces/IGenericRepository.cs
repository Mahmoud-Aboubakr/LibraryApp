using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllListAsync();
        Task<IReadOnlyList<T>> GetAllListWithIncludesAsync(Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllWithIncludesAsync(Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsyncWithIncludes(int id , Expression<Func<T, object>>[] includes);
        Task<T> FindAsync(Expression<Func<T,bool>> match, Expression<Func<T, object>>[] includes);
        Task<int> Complete();
        void DeleteAsync(T entity);
        void UpdateAsync(T entity);
        void InsertAsync(T entity);
        Task<int> SaveChangesAsync();
        Task<bool> Exists(int id);
    }
}
