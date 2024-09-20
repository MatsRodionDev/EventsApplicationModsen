using AutoMapper;
using EventsApplication.Application.Common.Interfaces.Repositories;
using EventsApplication.Application.Common.Interfaces.Services;
using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Application.Events.Commands.CreateEvent;
using EventsApplication.Domain.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace EventsApplication.Application.UnitTests.Events.Commands
{
    public class CreateEventCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateEventCommandHandler _handler;

        public CreateEventCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _fileServiceMock = new Mock<IFileService>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.EventRepository).Returns(_eventRepositoryMock.Object);
            _handler = new CreateEventCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _fileServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesEvent()
        {
            // Arrange
            var command = new CreateEventCommand(
                "Test Event",
                "This is a test event.",
                DateTime.UtcNow.AddDays(1),
                Guid.NewGuid(),
                "Test Category", 
                100,
                CreateMockFormFile("eventImage.png"));

            var userId = Guid.NewGuid();

            var newEvent = new Event();

            _mapperMock.Setup(m => m.Map<Event>(command)).Returns(newEvent);
            _fileServiceMock.Setup(fs => fs.SaveFilesync(command.Image!, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("some.png"); 

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.AddAsync(newEvent, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal("some.png", newEvent.EventImageName);
        }

        [Fact]
        public async Task Handle_ValidCommand_NoImage_CreatesEventWithoutImage()
        {
            // Arrange
            var command = new CreateEventCommand(
                "Test Event", 
                "This is a test event.", 
                DateTime.UtcNow.AddDays(1),
                Guid.NewGuid(), 
                "Test Category", 
                100, 
                null);

            var newEvent = new Event { Id = Guid.NewGuid() };

            _mapperMock.Setup(m => m.Map<Event>(command)).Returns(newEvent);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _eventRepositoryMock.Verify(repo => repo.AddAsync(newEvent, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Empty(newEvent.EventImageName); 
        }

        private IFormFile CreateMockFormFile(string fileName)
        {
            var stream = new MemoryStream([1, 2, 3]); 
            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(stream.Length);
            fileMock.Setup(f => f.ContentType).Returns("image/png");

            return fileMock.Object;
        }
    }
}
