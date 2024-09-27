using EventsApplication.Domain.Interfaces.Repositories;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Application.Subscriptions.Commands.DeleteSubscription;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using Moq;

namespace EventsApplication.Application.UnitTests.Subscriptions.Commands
{
    public class DeleteSubscriptionCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventSubscriptionRepository> _eventSubscriptionRepositoryMock;
        private readonly DeleteSubscriptionCommandHandler _handler;

        public DeleteSubscriptionCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventSubscriptionRepositoryMock = new Mock<IEventSubscriptionRepository>();

            _unitOfWorkMock.Setup(u => u.EventSubscriptionRepository).Returns(_eventSubscriptionRepositoryMock.Object);
            _handler = new DeleteSubscriptionCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_SubscriptionDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var command = new DeleteSubscriptionCommand(Guid.NewGuid(), Guid.NewGuid());

            _eventSubscriptionRepositoryMock.Setup(repo =>
                repo.GetSubscriptionsByUserIdAndEventIdAsync(command.UserId, command.EventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((EventSubscription)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal($"User doesn't have subscription with event with id {command.EventId}", exception.Message);
        }

        [Fact]
        public async Task Handle_SubscriptionExists_DeletesSubscription()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var eventId = Guid.NewGuid();

            var subscription = new EventSubscription
            {
                UserId = userId,
                EventId = eventId
            };

            var command = new DeleteSubscriptionCommand(eventId, userId); ;

            _eventSubscriptionRepositoryMock.Setup(repo =>
                repo.GetSubscriptionsByUserIdAndEventIdAsync(userId, eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subscription);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _eventSubscriptionRepositoryMock.Verify(repo => repo.Delete(subscription), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
