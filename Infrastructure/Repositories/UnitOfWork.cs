using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : BaseEntity
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IEntitySpec<T> _entitySpec;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(LibraryDbContext dbContext , IEntitySpec<T> entitySpec)
        {
            _dbContext = dbContext;
            _entitySpec = entitySpec;
            _repositories = new Dictionary<Type, object>();
        }

        public IGenericRepository<T> GetRepository()
        {
            if (_repositories.TryGetValue(typeof(T), out var repository))
            {
                return (IGenericRepository<T>)repository;
            }

            var newRepository = new GenericRepository<T>(_dbContext,_entitySpec);
            _repositories.Add(typeof(T), newRepository);
            return newRepository;
        }

        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
