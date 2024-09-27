using EventsApplication.Domain.Interfaces.Repositories;
using EventsApplication.Domain.Interfaces.UnitOfWork;
using EventsApplication.Application.Places.Commands.DeletePlace;
using EventsApplication.Domain.Exceptions;
using EventsApplication.Domain.Models;
using Moq;

namespace EventsApplication.Application.UnitTests.Places.Commands
{
    public class DeletePlaceCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPlaceRepository> _placeRepositoryMock;
        private readonly DeletePlaceCommandHandler _handler;

        public DeletePlaceCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _placeRepositoryMock = new Mock<IPlaceRepository>();

            _unitOfWorkMock.Setup(u => u.PlaceRepository).Returns(_placeRepositoryMock.Object);
            _handler = new DeletePlaceCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_PlaceExists_DeletesPlace()
        {
            // Arrange
            var placeId = Guid.NewGuid();
            var place = new Place { Id = placeId };

            _placeRepositoryMock.Setup(repo => repo.GetByIdAsync(placeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(place);

            // Act
            await _handler.Handle(new DeletePlaceCommand(placeId), CancellationToken.None);

            // Assert
            _placeRepositoryMock.Verify(repo => repo.Delete(place), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_PlaceDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var placeId = Guid.NewGuid();

            _placeRepositoryMock.Setup(repo => repo.GetByIdAsync(placeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Place)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(new DeletePlaceCommand(placeId), CancellationToken.None));

            Assert.Equal($"Place with id {placeId} doesn't exist.", exception.Message);
        }
    }
}
