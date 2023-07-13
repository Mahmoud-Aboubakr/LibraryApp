using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        void DeleteByIdAsync(int id);

        void DeleteAllAsync(T entity);

        void UpdateAsync(T entity);

        void InsertAsync(T entity);
        public Task<int> Complete();

        public Task<IEnumerable<T>> Search(Expression<Func<T, bool>> container);

        public Task<bool> Exists(int id);
    }
}
