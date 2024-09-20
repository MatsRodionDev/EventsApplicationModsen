using EventsApplication.Application.Common.Interfaces.Repositories;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Application.Events.Commands.DeleteEvent;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using Moq;

namespace EventsApplication.Application.UnitTests.Events.Commands
{
    public class DeleteEventCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IFileService> _fileService;
        private readonly DeleteEventCommandHandler _handler;

        public DeleteEventCommandHandlerTests()
        {
            _fileService = new Mock<IFileService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _unitOfWorkMock.Setup(u => u.EventRepository).Returns(_eventRepositoryMock.Object);

            _handler = new DeleteEventCommandHandler(_unitOfWorkMock.Object, _fileService.Object);
        }

        [Fact]
        public async Task Handle_ExistingEvent_DeletesEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var command = new DeleteEventCommand(eventId);
            var existingEvent = new Event { Id = eventId };

            Console.WriteLine(command.EventId);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.Delete(existingEvent), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingEvent_ThrowsNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var command = new DeleteEventCommand(eventId);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event)null!); 

            // Act Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal($"Event with id {eventId} doesn't exist", exception.Message);
        }
    }
}
