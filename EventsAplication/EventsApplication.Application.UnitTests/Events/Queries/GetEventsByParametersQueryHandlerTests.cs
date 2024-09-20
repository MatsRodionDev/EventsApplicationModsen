using EventsApplication.Application.Common.Interfaces.Repositories;
using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Application.Events.Queries.GetByParameters;
using EventsApplication.Domain.Enums;
using EventsApplication.Domain.Models;
using Microsoft.Extensions.Configuration;
using Moq;


namespace EventsApplication.Application.UnitTests.Events.Queries
{
    public class GetEventsByParametersQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly GetEventsByParametersQueryHandler _handler;

        public GetEventsByParametersQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _configurationMock = new Mock<IConfiguration>();

            _unitOfWorkMock.Setup(u => u.EventRepository).Returns(_eventRepositoryMock.Object);
            _handler = new GetEventsByParametersQueryHandler(_unitOfWorkMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task Handle_EventsExist_ReturnsEventsWithCorrectProperties()
        {
            // Arrange
            var queryParameters = new GetEventsByParametersQuery(DateTime.UtcNow,  EventCategory.Art, Guid.NewGuid(), "Name", 1, 1);

            var events = new List<Event>
            {
                new Event { Id = Guid.NewGuid(), EventTime = DateTime.UtcNow.AddHours(1), EventImageName = "image1.png" },
                new Event { Id = Guid.NewGuid(), EventTime = DateTime.UtcNow.AddHours(-1), EventImageName = "image2.png" },
                new Event { Id = Guid.NewGuid(), EventTime = DateTime.UtcNow.AddHours(2), EventImageName = string.Empty }
            };

            _eventRepositoryMock.Setup(repo => repo.GetEventsByQueryParametersAsync(queryParameters, It.IsAny<CancellationToken>()))
                .ReturnsAsync(events);

            _configurationMock.Setup(c => c["BaseAppUrl:BaseUrl"]).Returns("http://localhost");

            // Act
            var result = await _handler.Handle(queryParameters, CancellationToken.None);

            // Assert
            Assert.Equal(3, result.Count);

            Assert.False(result[0].IsEnded); 
            Assert.True(result[1].IsEnded);  
            Assert.False(result[2].IsEnded);

            Assert.Equal("http://localhost/image1.png", result[0].ImageUrl); 
            Assert.Equal("http://localhost/image2.png", result[1].ImageUrl);
            Assert.Empty(result[2].ImageUrl); 
        }

        [Fact]
        public async Task Handle_NoEventsFound_ReturnsEmptyList()
        {
            // Arrange
            var queryParameters = new GetEventsByParametersQuery(DateTime.UtcNow, EventCategory.Art, Guid.NewGuid(), "Name", 1, 1);

            _eventRepositoryMock.Setup(repo => repo.GetEventsByQueryParametersAsync(queryParameters, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Event>()); 

            // Act
            var result = await _handler.Handle(queryParameters, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
