using EventsApplication.Application.Common.Interfaces.Repositories;
using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Application.Subscriptions.Commands.CreateSubscriptionForRegisteredUser;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using Moq;

namespace EventsApplication.Application.UnitTests.Subscriptions.Commands
{
    public class CreateSubscriptionForRegisteredUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IEventSubscriptionRepository> _eventSubscriptionRepositoryMock;
        private readonly CreateSubscriptionForRegisteredUserCommandHandler _handler;

        public CreateSubscriptionForRegisteredUserCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _eventSubscriptionRepositoryMock = new Mock<IEventSubscriptionRepository>();

            _unitOfWorkMock.Setup(u => u.EventRepository).Returns(_eventRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.EventSubscriptionRepository).Returns(_eventSubscriptionRepositoryMock.Object);
            _handler = new CreateSubscriptionForRegisteredUserCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_EventDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var command = new CreateSubscriptionForRegisteredUserCommand(Guid.NewGuid(), Guid.NewGuid());

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(command.EventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event)null!); 

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal($"Event with id {command.EventId} doesn't exist", exception.Message);
        }

        [Fact]
        public async Task Handle_EventIsEnded_ThrowsBadRequestException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var command = new CreateSubscriptionForRegisteredUserCommand(Guid.NewGuid(), eventId);

            var @event = new Event
            {
                Id = eventId,
                EventTime = DateTime.UtcNow.AddHours(-1), 
                IsFool = false,
                MaxParticipants = 10
            };

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(@event);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal($"Event with id {eventId} is ended", exception.Message);
        }

        [Fact]
        public async Task Handle_EventIsFull_ThrowsBadRequestException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var command = new CreateSubscriptionForRegisteredUserCommand(Guid.NewGuid(), eventId);

            var @event = new Event
            {
                Id = eventId,
                EventTime = DateTime.UtcNow.AddHours(1),
                IsFool = true,
                MaxParticipants = 10
            };

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(@event);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal($"Event with id {eventId} is full", exception.Message);
        }

        [Fact]
        public async Task Handle_SuccessfulSubscription_CreatesSubscription()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var command = new CreateSubscriptionForRegisteredUserCommand(userId, eventId);

            var @event = new Event
            {
                Id = eventId,
                EventTime = DateTime.UtcNow.AddHours(1),
                IsFool = false,
                MaxParticipants = 2 
            };

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(@event);

            _eventSubscriptionRepositoryMock.Setup(repo => repo.GetSubscriptionCountAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); 

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _eventSubscriptionRepositoryMock.Verify(repo => repo
                .AddAsync(It
                    .Is<EventSubscription>(
                    s => s.UserId == userId 
                    && s.EventId == eventId), 
                        It.IsAny<CancellationToken>()), 
                        Times.Once);

            Assert.True(@event.IsFool); 

            _eventRepositoryMock.Verify(repo => repo.Update(@event), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
