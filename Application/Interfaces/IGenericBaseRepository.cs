using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IGenericBaseRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();  
        Task<T> GetByIdAsync(int id);       
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        Task<bool> Exists(int id);
        void DeleteAsync(T entity);
        void UpdateAsync(T entity);
        void InsertAsync(T entity);
        
    }
}
