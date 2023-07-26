using Domain.Entities;
using Infrastructure.Specifications;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<bool> Exists(int id);
        Task<bool> Exists(ISpecification<T> spec);
        void DeleteAsync(T entity);
        void DeleteRangeAsync(List<T> entities);
        void UpdateAsync(T entity);
        void InsertAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllListAsync();
        Task<T> FindSpec(ISpecification<T> spec);
        Task<IEnumerable<T>> FindAllSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllListWithIncludesAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> specification);      
    }
}
