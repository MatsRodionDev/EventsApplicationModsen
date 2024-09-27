using AutoMapper;
using EventsApplication.Domain.Enums;
using EventsApplication.Domain.Models;
using EventsApplication.Persistence.Entities;
using EventsApplication.Persistence.Repositories;
using EventsApplication.Persistence;
using Microsoft.EntityFrameworkCore;
using EventsApplication.Persistence.Profiles;


namespace EventsApplication.Persistense.UnitTests.Repositories
{
    public class EventRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDBContext> _options;
        private readonly IMapper _mapper;
        private readonly Guid PlaceId;

        public EventRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Event")
                .Options;

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PersistenceProfile());
            }).CreateMapper();

            using (var context = new ApplicationDBContext(_options))
            {
                PlaceId = Guid.NewGuid();

                context.Set<PlaceEntity>().AddRange(new List<PlaceEntity>
                {
                    new PlaceEntity { Id = PlaceId, Name = "Place 1" }
                });
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetEventsByQueryParametersAsync_ShouldReturnFilteredEvents()
        {
            // Arrange
            var event1 = new EventEntity
            {
                Id = Guid.NewGuid(),
                Name = "Gimn",
                EventTime = DateTime.Now.AddHours(1),
                PlaceId = PlaceId,
                Category = EventCategory.Music 
            };

            var event2 = new EventEntity
            {
                Id = Guid.NewGuid(),
                Name = "Theater",
                EventTime = DateTime.Now.AddHours(2),
                PlaceId = PlaceId,
                Category = EventCategory.Music
            };

            using (var context = new ApplicationDBContext(_options))
            {
                context.Set<EventEntity>().AddRange(event1, event2);
                context.SaveChanges();
            }

            // Act
            IEnumerable<Event> result;
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new EventRepository(context, _mapper);
                result = await repository.GetEventsWithPlacesAsync(CancellationToken.None);
            }

            // Assert
            Assert.Equal("Gimn", result.ToList()[0].Name);
        }

        [Fact]
        public async Task GetEventsByNameAsync_ShouldReturnMatchingEvents()
        {
            // Arrange
            var event1 = new EventEntity
            {
                Id = Guid.NewGuid(),
                Name = "Music",
                EventTime = DateTime.Now,
                PlaceId = PlaceId,
                Category = EventCategory.Music
            };

            using (var context = new ApplicationDBContext(_options))
            {
                context.Set<EventEntity>().Add(event1);
                context.SaveChanges();
            }

            // Act
            List<Event> result;
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new EventRepository(context, _mapper);
                result = await repository.GetEventsByNameAsync("Music", CancellationToken.None);
            }

            // Assert
            Assert.Single(result);
            Assert.Equal("Music", result[0].Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEvents()
        {
            // Arrange
            var id1 = Guid.NewGuid();
            var event1 = new EventEntity
            {
                Id = id1,
                Name = "Concert",
                EventTime = DateTime.Now,
                PlaceId = PlaceId,
                Category = EventCategory.Music
            };

            var id2 = Guid.NewGuid();
            var event2 = new EventEntity
            {
                Id = id2,
                Name = "Theater",
                EventTime = DateTime.Now.AddDays(1),
                PlaceId = PlaceId,
                Category = EventCategory.Music
            };

            using (var context = new ApplicationDBContext(_options))
            {
                context.Set<EventEntity>().AddRange(event1, event2);
                context.SaveChanges();
            }

            // Act
            List<Event> result;
            using (var context = new ApplicationDBContext(_options))
            {
                var repository = new EventRepository(context, _mapper);
                result = await repository.GetAllAsync(CancellationToken.None);
            }

            // Assert
            Assert.Contains(result, e => e.Id == id1);
            Assert.Contains(result, e => e.Id == id2);
        }
    }
}
