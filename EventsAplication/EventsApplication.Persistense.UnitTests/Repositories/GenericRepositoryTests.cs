using AutoMapper;
using EventsApplication.Domain.Models;
using EventsApplication.Persistence.Entities;
using EventsApplication.Persistence.Repositories;
using EventsApplication.Persistence;
using Microsoft.EntityFrameworkCore;
using EventsApplication.Persistence.Profiles;

namespace EventsApplication.Persistense.UnitTests.Repositories
{
    public class GenericRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDBContext> _options;
        private readonly IMapper _mapper;

        public GenericRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PersistenceProfile());
            }).CreateMapper();
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var id = Guid.NewGuid();

            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new GenericRepository<PlaceEntity, Place>(context, _mapper);
                var model = new Place { Id = id, Name = "Test Place" };

                // Act
                await repository.AddAsync(model, CancellationToken.None);
                await context.SaveChangesAsync(CancellationToken.None);
            }

            // Assert
            using (var context = new ApplicationDBContext(_options))
            {
                var entity = await context.Set<PlaceEntity>().FirstOrDefaultAsync(p => p.Id == id);
                Assert.NotNull(entity);
                Assert.Equal("Test Place", entity.Name);
            }
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            var model = new Place { Id = Guid.NewGuid(), Name = "Test Place" };
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new GenericRepository<PlaceEntity, Place>(context, _mapper);
                context.Set<PlaceEntity>().Add(new PlaceEntity { Id = model.Id, Name = model.Name });
                context.SaveChanges();
            }

            // Act
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new GenericRepository<PlaceEntity, Place>(context, _mapper);
                repository.Delete(model);
                context.SaveChanges();
            }

            // Assert
            using (var context = new ApplicationDBContext(_options))
            {
                var entity = await context.Set<PlaceEntity>().FindAsync(model.Id);
                Assert.Null(entity); // Подписка должна быть удалена
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllModels()
        {
            // Arrange
            using (var context = new ApplicationDBContext(_options))
            {
                context.Set<PlaceEntity>().AddRange(new List<PlaceEntity>
                {
                    new PlaceEntity { Id = Guid.NewGuid(), Name = "Place 1" },
                    new PlaceEntity { Id = Guid.NewGuid(), Name = "Place 2" }
                });
                context.SaveChanges();
            }

            // Act
            List<Place> result;
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new GenericRepository<PlaceEntity, Place>(context, _mapper);
                result = await repository.GetAllAsync(CancellationToken.None);
            }

            // Assert
            Assert.Contains(result, r => r.Name == "Place 1");
            Assert.Contains(result, r => r.Name == "Place 2");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnModel()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new PlaceEntity { Id = id, Name = "Test Place" };
            using (var context = new ApplicationDBContext(_options))
            {
                context.Set<PlaceEntity>().Add(entity);
                context.SaveChanges();
            }

            // Act
            Place result;
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new GenericRepository<PlaceEntity, Place>(context, _mapper);
                result = await repository.GetByIdAsync(id, CancellationToken.None);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Place", result.Name);
        }

        [Fact]
        public void Update_ShouldModifyEntity()
        {
            // Arrange
            var model = new Place { Id = Guid.NewGuid(), Name = "Original Place" };
            using (var context = new ApplicationDBContext(_options))
            {
                context.Set<PlaceEntity>().Add(new PlaceEntity { Id = model.Id, Name = model.Name });
                context.SaveChanges();
            }

            // Act
            var updatedModel = new Place { Id = model.Id, Name = "Updated Place" };
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new GenericRepository<PlaceEntity, Place>(context, _mapper);
                repository.Update(updatedModel);
                context.SaveChanges();
            }

            // Assert
            using (var context = new ApplicationDBContext(_options))
            {
                var entity = context.Set<PlaceEntity>().Find(model.Id);
                Assert.NotNull(entity);
                Assert.Equal("Updated Place", entity.Name);
            }
        }
    }
}
