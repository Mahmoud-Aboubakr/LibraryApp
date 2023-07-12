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

        public void DeleteAllAsync(T entity)
            => _context.Set<T>().Remove(entity);

        //public async Task DeleteAllAsyncWithIncludes(/*T entity*/ Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[] includes)
        //{
        //    //var query = _context.Set<T>().AsQueryable();
        //    //if (includes != null)
        //    //{
        //    //    query = includes.Aggregate(query, (current, include) => current.Include(include));
        //    //}
        //    //_context.Remove(query);

        //    var query = _context.Set<T>().AsQueryable().Where(predicate);
       
        //    foreach (var include in includes)
        //    {
        //        query = query.Include(include);
        //    }

        //    var entities = await query.ToListAsync();

        //    if (entities != null && entities.Any())
        //    {
        //        _context.Set<T>().RemoveRange(entities);
        //    }
        //}

        public void DeleteByIdAsync(int id)
        {
            T existing = _entity.Find(id);

            if (existing != null)
                _entity.Remove(existing);

        }

        //public async Task DeleteByIdAsyncWithIncludes(int id, Expression<Func<T, bool>> predicate ,Expression<Func<T, object>>[] includes)
        //{
        //    var query = _context.Set<T>().AsQueryable();
        //    if (includes != null)
        //    {
        //        query = includes.Aggregate(query, (current, include) => current.Include(include));
        //    }
        //    var entity = await query.FirstOrDefaultAsync(e => e.Id == id);

        //    _context.Set<T>().Remove(entity);

        //}

        public Task<T> FindAsync(Expression<Func<T, bool>> match, Expression<Func<T, object>>[] includes)
        {

            var query = _context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query.FirstOrDefaultAsync(match);
        }

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

        public void InsertAsync(T entity)
            => _context.Set<T>().Add(entity);

        //public void InsertAsyncWithIncludes(T entity, Expression<Func<T, object>>[] includes)
        //{
        //    throw new NotImplementedException();
        //}

        public void UpdateAsync(T entity)
            => _context.Set<T>().Update(entity);

        //public void UpdateAsyncWithIncludes(T entity, Expression<Func<T, object>>[] includes)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
