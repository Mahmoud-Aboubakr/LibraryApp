using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();  
        Task<T> GetByIdAsync(int id);       
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        Task<bool> Exists(int id);
        void DeleteAsync(T entity);
        void UpdateAsync(T entity);
        void InsertAsync(T entity);
        Task<IReadOnlyList<T>> GetAllListWithIncludesAsync(Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllWithIncludesAsync(Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsyncWithIncludes(int id, Expression<Func<T, object>>[] includes);
        Task<IReadOnlyList<T>> GetAllListAsync();
        Task<T> FindAsync(Expression<Func<T, bool>> match, Expression<Func<T, object>>[] includes);
    }
}
