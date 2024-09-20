using EventsApplication.Application.Common.Interfaces.Repositories;
using EventsApplication.Application.Common.Interfaces.UnitOfWork;
using EventsApplication.Application.Places.Queries.GetAll;
using EventsApplication.Domain.Models;
using Moq;

namespace EventsApplication.Application.UnitTests.Places.Queries
{
    public class GetAllPlacesQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPlaceRepository> _placeRepositoryMock;
        private readonly GetAllPlacesQueryHandler _handler;

        public GetAllPlacesQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _placeRepositoryMock = new Mock<IPlaceRepository>();

            _unitOfWorkMock.Setup(u => u.PlaceRepository).Returns(_placeRepositoryMock.Object);
            _handler = new GetAllPlacesQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_PlacesExist_ReturnsListOfPlaces()
        {
            // Arrange
            var places = new List<Place>
            {
                new Place { Id = Guid.NewGuid(), Name = "Place 1"},
                new Place { Id = Guid.NewGuid(), Name = "Place 2"}
            };

            _placeRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(places);

            // Act
            var result = await _handler.Handle(new GetAllPlacesQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Place 1", result[0].Name);
            Assert.Equal("Place 2", result[1].Name);
        }

        [Fact]
        public async Task Handle_NoPlacesExist_ReturnsEmptyList()
        {
            // Arrange
            _placeRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Place>());

            // Act
            var result = await _handler.Handle(new GetAllPlacesQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
