using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
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
        private DbSet<T> _entity = null;

        public GenericRepository(LibraryDbContext context)
        {
            _entity = context.Set<T>();
        }
        public GenericRepository()
        {

        }

        #region GET Methods
        public async Task<List<T>> GetAllAsync()
            => await _entity.ToListAsync();
        public async Task<T> GetByIdAsync(int id)
        {
            return await _entity.FindAsync(id);
        }
        public async Task<IReadOnlyList<T>> GetAllListAsync()
            => await _entity.ToListAsync();
        public async Task<IReadOnlyList<T>> GetAllListWithIncludesAsync(Expression<Func<T, object>>[] includes)
        {
            var query = _entity.AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return await query.ToListAsync();
        }
        public async Task<List<T>> GetAllWithIncludesAsync(Expression<Func<T, object>>[] includes)
        {
            var query = _entity.AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return await query.ToListAsync();
        }
        public async Task<T> GetByIdAsyncWithIncludes(int id, Expression<Func<T, object>>[] includes)
        {
            var query = _entity.AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<T>> GetAllWithWhere(Expression<Func<T, bool>> predicate)
        {
            return await _entity.Where(predicate).ToListAsync();
        }

        #endregion

        #region FIND Methods
        public Task<T> FindAsync(Expression<Func<T, bool>> match, Expression<Func<T, object>>[] includes)
        {
            var query = _entity.AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query.FirstOrDefaultAsync(match);
        }
        public Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            var query = _entity.AsQueryable();
            return query.FirstOrDefaultAsync(match);
        }
        public async Task<bool> FindUsingWhereAsync(Expression<Func<T, bool>> match)
        {
            var query = _entity;
            var result = await query.AnyAsync(match);

            return result;
        }
        public async Task<bool> Exists(int id)
        {
            return await _entity.AnyAsync(x => x.Id == id);
        }
        public async Task<T> FindSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> FindAllSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        #endregion

        #region INSERT Methods
        public void InsertAsync(T entity)
           => _entity.Add(entity);
        #endregion

        #region UPDATE Methods
        public void UpdateAsync(T entity)
           => _entity.Update(entity);
        #endregion

        #region DELETE Methods
        public void DeleteAsync(T entity)
           => _entity.Remove(entity);

        public void DeleteRangeAsync(List<T> entities)
        {
            _entity.RemoveRange(entities);
        }

        #endregion

        #region Specification Methods       
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_entity.AsQueryable(), spec);
        }
        #endregion

        #region Count
        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }
        #endregion

    }
}
