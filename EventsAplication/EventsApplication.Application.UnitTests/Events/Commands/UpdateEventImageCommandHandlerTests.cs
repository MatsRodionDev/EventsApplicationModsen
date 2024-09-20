using EventsApplication.Application.Common.Interfaces.Repositories;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Application.Events.Commands.UpdateEventImage;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace EventsApplication.Application.UnitTests.Events.Commands
{
    public class UpdateEventImageCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly UpdateEventImageCommandHandler _handler;

        public UpdateEventImageCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _fileServiceMock = new Mock<IFileService>();

            _unitOfWorkMock.Setup(u => u.EventRepository).Returns(_eventRepositoryMock.Object);

            _handler = new UpdateEventImageCommandHandler(_unitOfWorkMock.Object, _fileServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingEvent_UpdatesImage()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var imageName = "newImage.png"; 
            var command = new UpdateEventImageCommand(eventId, CreateMockFormFile(imageName)); 
            var existingEvent = new Event { Id = eventId, EventImageName = "oldImage.png" };

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEvent);

            _fileServiceMock.Setup(fs => fs.Delete("oldImage.png"));
            _fileServiceMock.Setup(fs => fs.SaveFilesync(It.IsAny<IFormFile>(), eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(imageName); 

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _fileServiceMock.Verify(fs => fs.Delete("oldImage.png"), Times.Once);
            _fileServiceMock.Verify(fs => fs.SaveFilesync(It.IsAny<IFormFile>(), eventId, It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(imageName, existingEvent.EventImageName);
            _eventRepositoryMock.Verify(repo => repo.Update(existingEvent), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingEvent_ThrowsNotFoundException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var command = new UpdateEventImageCommand(eventId, CreateMockFormFile("image.png"));

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Event)null!); 

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal($"Event with id {eventId} doesn't exist", exception.Message);
        }

        private IFormFile CreateMockFormFile(string fileName)
        {
            var stream = new MemoryStream(new byte[] { 1, 2, 3 }); 
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(stream.Length);
            fileMock.Setup(f => f.ContentType).Returns("image/png");

            return fileMock.Object;
        }
    }
}
