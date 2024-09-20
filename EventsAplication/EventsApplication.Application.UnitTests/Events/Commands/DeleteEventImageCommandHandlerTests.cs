using EventsApplication.Application.Common.Interfaces.Repositories;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Application.Events.Commands.DeleteEventImage;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using Moq;


namespace EventsApplication.Application.UnitTests.Events.Commands
{
    public class DeleteEventImageCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly DeleteEventImageCommandHandler _handler;

        public DeleteEventImageCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _fileServiceMock = new Mock<IFileService>();

            _unitOfWorkMock.Setup(u => u.EventRepository).Returns(_eventRepositoryMock.Object);

            _handler = new DeleteEventImageCommandHandler(_unitOfWorkMock.Object, _fileServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingEvent_DeletesImage()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var command = new DeleteEventImageCommand(eventId);
            var existingEvent = new Event { Id = eventId, EventImageName = "image.png" };

            _eventRepositoryMock.Setup(repo => repo
                .GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _fileServiceMock.Verify(fs => fs.Delete("image.png"), Times.Once);

            Assert.Equal(string.Empty, existingEvent.EventImageName);

            _eventRepositoryMock.Verify(repo => repo.Update(existingEvent), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingEvent_ThrowsNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var command = new DeleteEventImageCommand(eventId);

            _eventRepositoryMock.Setup(repo => repo
                .GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event)null!); 

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal($"Event with id {eventId} doesn't exist", exception.Message);
        }

        [Fact]
        public async Task Handle_NonExistingEventImage_ThrowsBadRequestException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var command = new DeleteEventImageCommand(eventId);

            var @event = new Event { Id = eventId };

            _eventRepositoryMock.Setup(repo => repo
                .GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(@event);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal($"Event with id {@event.Id} doesn't have image", exception.Message);
        }
    }
}
