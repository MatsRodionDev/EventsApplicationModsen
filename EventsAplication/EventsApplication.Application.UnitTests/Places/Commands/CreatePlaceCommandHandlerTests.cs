using AutoMapper;
using EventsApplication.Domain.Interfaces.Repositories;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Application.Places.Commands.CreatePlace;
using EventsApplication.Domain.Models;
using Moq;

namespace EventsApplication.Application.UnitTests.Places.Commands
{
    public class CreatePlaceCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPlaceRepository> _placeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreatePlaceCommandHandler _handler;

        public CreatePlaceCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _placeRepositoryMock = new Mock<IPlaceRepository>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.PlaceRepository).Returns(_placeRepositoryMock.Object);
            _handler = new CreatePlaceCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesNewPlace()
        {
            // Arrange
            var command = new CreatePlaceCommand("Test Place");

            var newPlace = new Place
            {
                Id = Guid.NewGuid(),
                Name = command.Name
            };

            _mapperMock.Setup(m => m.Map<Place>(command)).Returns(newPlace);


            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, newPlace.Id); 

            _placeRepositoryMock.Verify(repo => repo.AddAsync(newPlace, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
