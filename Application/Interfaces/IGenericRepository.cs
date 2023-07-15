using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllListWithIncludesAsync(Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllWithIncludesAsync(Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsyncWithIncludes(int id, Expression<Func<T, object>>[] includes);
        Task<IReadOnlyList<T>> GetAllListAsync();
        Task<T> FindAsync(Expression<Func<T, bool>> match, Expression<Func<T, object>>[] includes);



        Task<int> SaveChangesAsync();
        Task<int> Complete();
        
    }
}
