using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly LibraryDbContext _context;
        private DbSet<T> _entity = null;

        public GenericRepository(LibraryDbContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }
        
        #region GET Methods
        public async Task<List<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();
        public async Task<IReadOnlyList<T>> GetAllListAsync()
            => await _context.Set<T>().ToListAsync();
        public async Task<IReadOnlyList<T>> GetAllListWithIncludesAsync(Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return await query.ToListAsync();
        }
        public async Task<List<T>> GetAllWithIncludesAsync(Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return await query.ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<T> GetByIdAsyncWithIncludes(int id, Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }
        #endregion

        #region FIND Methods
        public Task<T> FindAsync(Expression<Func<T, bool>> match, Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query.FirstOrDefaultAsync(match);
        }
        public async Task<bool> Exists(int id)
        {
            return await _context.Set<T>().AnyAsync(x => x.Id == id);
        }
        #endregion

        #region INSERT Methods
        public void InsertAsync(T entity)
           => _context.Set<T>().Add(entity);
        #endregion

        #region UPDATE Methods
        public void UpdateAsync(T entity)
           => _context.Set<T>().Update(entity);
        #endregion

        #region DELETE Methods
        public void DeleteAsync(T entity)
           => _context.Set<T>().Remove(entity);
        #endregion

        #region Save Changes Method
        public async Task<int> Complete()
           => await _context.SaveChangesAsync();
       
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
