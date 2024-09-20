using AutoMapper;
using EventsApplication.Application.Common.Interfaces.Queues;
using EventsApplication.Application.Common.Interfaces.Repositories;
using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Application.Common.Profiles;
using EventsApplication.Application.Events.Commands.UpdateEvent;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using Moq;

namespace EventsApplication.Application.UnitTests.Events.Commands
{
    public class UpdateEventCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IEventSubscriptionRepository> _eventSubscriptionRepositoryMock;
        private readonly IMapper _mapper;
        private readonly UpdateEventCommandHandler _handler;
        private readonly Mock<IEventUpdateQueue> _queue;

        public UpdateEventCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _eventSubscriptionRepositoryMock = new Mock<IEventSubscriptionRepository>();
            _queue = new Mock<IEventUpdateQueue>();

            _unitOfWorkMock.Setup(u => u.EventRepository).Returns(_eventRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.EventSubscriptionRepository).Returns(_eventSubscriptionRepositoryMock.Object);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            _mapper = config.CreateMapper();

            _handler = new UpdateEventCommandHandler(_unitOfWorkMock.Object, _mapper, _queue.Object);
        }

        [Fact]
        public async Task Handle_ValidData_UpdatesEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var placeId = Guid.NewGuid();
            var command = new UpdateEventCommand(eventId, "Updated Name", "Updated Description", DateTime.Now, placeId, "Food", 100);

            _eventSubscriptionRepositoryMock.Setup(repo => repo
                .GetSubscriptionCountAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(50);

            _eventRepositoryMock.Setup(repo => repo.Update(It.IsAny<Event>()));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _eventRepositoryMock.Verify(repo => repo
            .Update(It.Is<Event>(e => e.Id == eventId 
                    && e.MaxParticipants == 100)), 
                Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_TooManyParticipants_ThrowsBadRequestException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var placeId = Guid.NewGuid();
            var command = new UpdateEventCommand(eventId, "Updated Name", "Updated Description", DateTime.Now, placeId, "Food", 20);

            _eventSubscriptionRepositoryMock.Setup(repo => repo
                .GetSubscriptionCountAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(30); 

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("There are already 30 participants. MaxParticipants cannot be less than the number of participants", exception.Message);
        }
    }
}
