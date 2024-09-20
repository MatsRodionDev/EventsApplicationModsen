using AutoMapper;
using EventsApplication.Domain.Models;
using EventsApplication.Persistence.Entities;
using EventsApplication.Persistence.Repositories;
using EventsApplication.Persistence;
using Microsoft.EntityFrameworkCore;
using EventsApplication.Persistence.Profiles;

namespace EventsApplication.Persistense.UnitTests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDBContext> _options;
        private readonly IMapper _mapper;

        public UserRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_User")
                .Options;

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PersistenceProfile());
            }).CreateMapper();
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnUser_WhenEmailExists()
        {
            // Arrange
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            using (var context = new ApplicationDBContext(_options))
            {
                context.Set<UserEntity>().Add(userEntity);
                await context.SaveChangesAsync();
            }

            // Act
            User result;
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new UserRepository(context, _mapper);
                result = await repository.GetUserByEmailAsync("test@example.com", CancellationToken.None);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            // Arrange
            using (var context = new ApplicationDBContext(_options))
            {
                var userEntity = new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Email = "teeest@example.com",
                    FirstName = "John",
                    LastName = "Doe"
                };
            }

            // Act
            User result;
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new UserRepository(context, _mapper);
                result = await repository.GetUserByEmailAsync("nonexistent@example.com", CancellationToken.None);
            }

            // Assert
            Assert.Null(result);
        }
    }
}
