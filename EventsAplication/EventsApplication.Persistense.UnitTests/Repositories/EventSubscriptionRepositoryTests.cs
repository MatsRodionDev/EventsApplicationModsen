using AutoMapper;
using EventsApplication.Domain.Models;
using EventsApplication.Persistence.Entities;
using EventsApplication.Persistence.Repositories;
using EventsApplication.Persistence;
using Microsoft.EntityFrameworkCore;
using EventsApplication.Persistence.Profiles;

namespace EventsApplication.Persistense.UnitTests.Repositories
{
    public class EventSubscriptionRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDBContext> _options;
        private readonly IMapper _mapper;
        private readonly Guid PlaceId;

        public EventSubscriptionRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_EventSubscription")
                .Options;

            using (var context = new ApplicationDBContext(_options))
            {
                PlaceId = Guid.NewGuid();

                context.Set<PlaceEntity>().AddRange(new List<PlaceEntity>
                {
                    new PlaceEntity { Id = PlaceId, Name = "Place 1" }
                });
                context.SaveChanges();
            }

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PersistenceProfile());
            }).CreateMapper();
        }

        [Fact]
        public async Task GetSubscriptionsWithUsersByEventIdAsync_ShouldReturnSubscriptions_WhenEventExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var eventId = Guid.NewGuid();

            var userEntity = new UserEntity { Id = userId, Email = "test@example.com" };
            var eventEntity = new EventEntity { Id = eventId, Name = "Concert" };
            var subscriptionEntity = new EventSubscriptionEntity { UserId = userId, EventId = eventId, User = userEntity, Event = eventEntity };

            using (var context = new ApplicationDBContext(_options))
            {
                context.Set<UserEntity>().Add(userEntity);
                context.Set<EventEntity>().Add(eventEntity);
                context.Set<EventSubscriptionEntity>().Add(subscriptionEntity);
                await context.SaveChangesAsync();
            }

            // Act
            List<EventSubscription> result;
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new EventSubscriptionRepository(context, _mapper);
                result = await repository.GetSubscriptionsWithUsersByEventIdAsync(eventId, CancellationToken.None);
            }

            // Assert
            Assert.Single(result);
            Assert.Equal(userId, result[0].UserId);
            Assert.Equal(eventId, result[0].EventId);
        }

        [Fact]
        public async Task GetSubscriptionsWithEventsByUserIdAsync_ShouldReturnSubscriptions_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var eventId = Guid.NewGuid();

            var userEntity = new UserEntity { Id = userId, Email = "tsest@example.com" };
            var eventEntity = new EventEntity { Id = eventId, Name = "Cooncert", PlaceId = PlaceId };
            var subscriptionEntity = new EventSubscriptionEntity { UserId = userId, EventId = eventId, User = userEntity, Event = eventEntity };

            using (var context = new ApplicationDBContext(_options))
            {
                context.Set<UserEntity>().Add(userEntity);
                context.Set<EventEntity>().Add(eventEntity);
                context.Set<EventSubscriptionEntity>().Add(subscriptionEntity);
                await context.SaveChangesAsync();
            }

            // Act
            List<EventSubscription> result;
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new EventSubscriptionRepository(context, _mapper);
                result = await repository.GetSubscriptionsWithEventsByUserIdAsync(userId, CancellationToken.None);
            }

            // Assert
            Assert.Single(result);
            Assert.Equal(eventId, result[0].EventId);
            Assert.Equal(userId, result[0].UserId);
        }

        [Fact]
        public async Task GetSubscriptionsByUserIdAndEventIdAsync_ShouldReturnSubscription_WhenExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var eventId = Guid.NewGuid();

            var userEntity = new UserEntity { Id = userId, Email = "test@example.com" };
            var eventEntity = new EventEntity { Id = eventId, Name = "Concert" };
            var subscriptionEntity = new EventSubscriptionEntity { UserId = userId, EventId = eventId, User = userEntity, Event = eventEntity };

            using (var context = new ApplicationDBContext(_options))
            {
                context.Set<UserEntity>().Add(userEntity);
                context.Set<EventEntity>().Add(eventEntity);
                context.Set<EventSubscriptionEntity>().Add(subscriptionEntity);
                await context.SaveChangesAsync();
            }

            // Act
            EventSubscription? result;
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new EventSubscriptionRepository(context, _mapper);
                result = await repository.GetSubscriptionsByUserIdAndEventIdAsync(userId, eventId, CancellationToken.None);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(eventId, result.EventId);
        }

        [Fact]
        public async Task GetSubscriptionCountAsync_ShouldReturnCount_WhenSubscriptionsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var eventId = Guid.NewGuid();
            var subscriptionEntity = new EventSubscriptionEntity { UserId = userId, EventId = eventId };

            using (var context = new ApplicationDBContext(_options))
            {
                context.Set<EventSubscriptionEntity>().Add(subscriptionEntity);
                await context.SaveChangesAsync();
            }

            // Act
            int count;
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new EventSubscriptionRepository(context, _mapper);
                count = await repository.GetSubscriptionCountAsync(eventId, CancellationToken.None);
            }

            // Assert
            Assert.Equal(1, count);
        }
    }
}
