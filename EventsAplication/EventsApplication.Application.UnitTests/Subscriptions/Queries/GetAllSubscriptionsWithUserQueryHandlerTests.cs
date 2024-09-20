﻿using EventsApplication.Application.Common.Interfaces.Repositories;
using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Application.Subscriptions.Queries.GetAllSubscriptionsWithUser;
using EventsApplication.Domain.Models;
using Moq;

namespace EventsApplication.Application.UnitTests.Subscriptions.Queries
{
    public class GetAllSubscriptionsWithUserQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventSubscriptionRepository> _eventSubscriptionRepositoryMock;
        private readonly GetAllSubscriptionsWithUserQueryHandler _handler;

        public GetAllSubscriptionsWithUserQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventSubscriptionRepositoryMock = new Mock<IEventSubscriptionRepository>();

            _unitOfWorkMock.Setup(u => u.EventSubscriptionRepository).Returns(_eventSubscriptionRepositoryMock.Object);
            _handler = new GetAllSubscriptionsWithUserQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_SubscriptionsExist_ReturnsListOfSubscriptions()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var subscriptions = new List<EventSubscription>
            {
                new EventSubscription { UserId = Guid.NewGuid(), EventId = eventId, RegisterDate = DateTime.UtcNow },
                new EventSubscription { UserId = Guid.NewGuid(), EventId = eventId, RegisterDate = DateTime.UtcNow }
            };

            _eventSubscriptionRepositoryMock.Setup(repo =>
                repo.GetSubscriptionsWithUsersByEventIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subscriptions);

            // Act
            var result = await _handler.Handle(new GetAllSubscriptionsWithUserQuery(eventId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, sub => Assert.Equal(eventId, sub.EventId));
        }

        [Fact]
        public async Task Handle_NoSubscriptionsExist_ReturnsEmptyList()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            _eventSubscriptionRepositoryMock.Setup(repo =>
                repo.GetSubscriptionsWithUsersByEventIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EventSubscription>());

            // Act
            var result = await _handler.Handle(new GetAllSubscriptionsWithUserQuery(eventId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
