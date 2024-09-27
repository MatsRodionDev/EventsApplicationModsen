using EventsApplication.Domain.Interfaces.Repositories;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using EventsApplication.Application.UseCases.Handlers.Queries.Events;
using EventsApplication.Application.UseCases.Queries.Events;

namespace EventsApplication.Application.UnitTests.Events.Queries
{
    public class GetEventByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly GetEventByIdQueryHandler _handler;

        public GetEventByIdQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _configurationMock = new Mock<IConfiguration>();

            _unitOfWorkMock.Setup(u => u.EventRepository).Returns(_eventRepositoryMock.Object);
            _handler = new GetEventByIdQueryHandler(_unitOfWorkMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task Handle_EventExists_ReturnsEventWithCorrectProperties()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventEntity = new Event
            {
                Id = eventId,
                EventTime = DateTime.UtcNow.AddHours(1),
                EventImageName = "image1.png"
            };

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventEntity);

            _configurationMock.Setup(c => c["BaseAppUrl:BaseUrl"]).Returns("http://localhost");

            var query = new GetEventByIdQuery(eventId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventId, result.Id);
            Assert.Equal("http://localhost/image1.png", result.ImageUrl); 
            Assert.False(result.IsEnded); 
        }

        [Fact]
        public async Task Handle_EventDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event)null!);

            var query = new GetEventByIdQuery(eventId);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));

            Assert.Equal($"Event with Id {eventId} didn't find", exception.Message);
        }
    }
}
