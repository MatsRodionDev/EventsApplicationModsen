using EventsApplication.Domain.Interfaces.Repositories;
using EventsApplication.Domain.Interfaces.UnitOfWork;
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
        public async Task Handle_NoEventsFound_ReturnsEmptyList()
        {
            // Arrange
            var queryParameters = new GetEventsByParametersQuery(DateTime.UtcNow, EventCategory.Art, Guid.NewGuid(), "Name", 1, 1);

            _eventRepositoryMock.Setup(repo => repo.GetEventsWithPlacesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Event>()); 

            // Act
            var result = await _handler.Handle(queryParameters, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
