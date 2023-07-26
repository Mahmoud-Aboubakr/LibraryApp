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
        public async Task<IReadOnlyList<T>> GetAllListWithIncludesAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        #endregion

        #region FIND Methods
        public async Task<bool> Exists(int id)
        {
            return await _entity.AnyAsync(x => x.Id == id);
        }
        public async Task<bool> Exists(ISpecification<T> spec)
        {
            var result = await ApplySpecification(spec).SingleOrDefaultAsync();
            if (result != null)
                return true;
            else
                return false;
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
