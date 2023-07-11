using Domain.Entities;
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
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> FindAsync(Expression<Func<T,bool>> match, Expression<Func<T, object>>[] includes);
        void DeleteByIdAsync(int id);
        void DeleteAllAsync(T entity);
        void UpdateAsync(T entity);
        void InsertAsync(T entity);
    }
}
