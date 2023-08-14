using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Context;
using Persistence.Repositories;

namespace API.Test
{
    public class GenericRepositoryTest
    {
        private readonly Mock<LibraryDbContext> _dbContextMock;
        private readonly IGenericRepository<Author> _repository;
        private readonly List<Author> _entities;

        public GenericRepositoryTest()
        {
            _dbContextMock = new Mock<LibraryDbContext>();
            _repository = new GenericRepository<Author>();

            // Initialize sample data
            _entities = new List<Author>
        {
            new Author { Id = 1, AuthorName = "Entity 1",AuthorPhoneNumber="01006209613" },
            new Author { Id = 2, AuthorName = "Entity 2",AuthorPhoneNumber="01006209613" }
        };
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            //// Arrange
            //int entityId = 2;
            //var entity = _entities.FirstOrDefault(e => e.Id == entityId);
            //_dbContextMock.Setup(db => db.Set<Author>()).Returns(MockDbSet(_entities));

            //// Act
            //var result = await _repository.GetByIdAsync(entityId);

            //// Assert
            //Assert.Equal(entity, result);

            // Arrange
            var testObject = new Author();

            var context = new Mock<LibraryDbContext>();
            var dbSetMock = new Mock<DbSet<Author>>();

            context.Setup(x => x.Set<Author>()).Returns(dbSetMock.Object);
            dbSetMock.Setup(x => x.Find(It.IsAny<int>())).Returns(testObject);

            // Act
            var repository = new GenericRepository<Author>(context.Object);
            await repository.GetByIdAsync(1);

            // Assert
            context.Verify(x => x.Set<Author>());
            dbSetMock.Verify(x => x.Find(It.IsAny<int>()));
        }

        // Helper method to mock DbSet
        private static DbSet<T> MockDbSet<T>(IEnumerable<T> data) where T : BaseEntity
        {
            var queryableData = data.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>(MockBehavior.Strict);

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            //dbSetMock.As<IAsyncEnumerable<T>>()
            //    .Setup(m => m.GetAsyncEnumerator(default))
            //    .Returns(new TestAsyncEnumerator<T>(queryableData.GetEnumerator()));

            dbSetMock.Setup(d => d.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                .Returns((T entity, CancellationToken _) =>
                {
                    data = data.Append(entity);
                    return Task.CompletedTask;
                });

            dbSetMock.Setup(d => d.Update(It.IsAny<T>()))
                .Callback((T entity) =>
                {
                    var existingEntity = data.FirstOrDefault(e => e.Equals(entity));
                    if (existingEntity != null)
                    {
                        data = data.Except(new[] { existingEntity }).Append(entity);
                    }
                });

            dbSetMock.Setup(d => d.Remove(It.IsAny<T>()))
                .Callback((T entity) => { data = data.Except(new[] { entity }); });

            return dbSetMock.Object;
        }
    }
}