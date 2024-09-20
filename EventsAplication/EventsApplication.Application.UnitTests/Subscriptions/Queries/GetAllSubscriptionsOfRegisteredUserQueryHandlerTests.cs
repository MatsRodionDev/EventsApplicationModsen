using EventsApplication.Application.Common.Interfaces.Repositories;
using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Application.Subscriptions.Queries.GetAllSubscriptionsOfRegisteredUser;
using EventsApplication.Domain.Models;
using Moq;

namespace EventsApplication.Application.UnitTests.Subscriptions.Queries
{
    public class GetAllSubscriptionsOfRegisteredUserQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventSubscriptionRepository> _eventSubscriptionRepositoryMock;
        private readonly GetAllSubscriptionsOfRegisteredUserQueryHandler _handler;

        public GetAllSubscriptionsOfRegisteredUserQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventSubscriptionRepositoryMock = new Mock<IEventSubscriptionRepository>();

            _unitOfWorkMock.Setup(u => u.EventSubscriptionRepository).Returns(_eventSubscriptionRepositoryMock.Object);
            _handler = new GetAllSubscriptionsOfRegisteredUserQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_SubscriptionsExist_ReturnsListOfSubscriptions()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var subscriptions = new List<EventSubscription>
            {
                new EventSubscription { UserId = userId, EventId = Guid.NewGuid(), RegisterDate = DateTime.UtcNow },
                new EventSubscription { UserId = userId, EventId = Guid.NewGuid(), RegisterDate = DateTime.UtcNow }
            };

            _eventSubscriptionRepositoryMock.Setup(repo =>
                repo.GetSubscriptionsWithEventsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subscriptions);

            // Act
            var result = await _handler.Handle(new GetAllSubscriptionsOfRegisteredUserQuery(userId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(userId, result[0].UserId);
            Assert.Equal(userId, result[1].UserId);
        }

        [Fact]
        public async Task Handle_NoSubscriptionsExist_ReturnsEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _eventSubscriptionRepositoryMock.Setup(repo =>
                repo.GetSubscriptionsWithEventsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EventSubscription>());

            // Act
            var result = await _handler.Handle(new GetAllSubscriptionsOfRegisteredUserQuery(userId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); 
        }
    }
}
