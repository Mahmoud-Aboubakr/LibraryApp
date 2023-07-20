using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly LibraryDbContext _context;
        private readonly IEntitySpec<T> _entitySpec;
        private DbSet<T> _entity = null;

        public GenericRepository(LibraryDbContext context, IEntitySpec<T> entitySpec)
        {
            _context = context;
            _entity = _context.Set<T>();
            _entitySpec = entitySpec;
        }
        public GenericRepository()
        {
           
        }

        #region GET Methods
        public async Task<List<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
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
        public async Task<T> GetByIdAsyncWithIncludes(int id, Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<T>> GetAllWithWhere(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
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
        public Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            var query = _context.Set<T>().AsQueryable();
            return query.FirstOrDefaultAsync(match);
        }
        public async Task<bool> FindUsingWhereAsync(Expression<Func<T, bool>> match)
        {
            var query = _context.Set<T>();
            var result = await query.AnyAsync(match);

            return result;
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

        public void DeleteRangeAsync(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        #endregion

        #region Specification Methods
        public async Task<T> FindSpec(IEntitySpec<T> spec)
        {
            return await ApplySpecification(spec).SingleOrDefaultAsync();
        }
        private IQueryable<T> ApplySpecification(IEntitySpec<T> spec)
        {
            return _entitySpec.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        public async Task<IEnumerable<T>> FindAllSpec(IEntitySpec<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        #endregion

    }
}
