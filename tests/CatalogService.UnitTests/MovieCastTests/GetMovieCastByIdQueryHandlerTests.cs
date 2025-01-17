using AutoMapper;
using Catalog.Application.Features.MovieCast.Handlers.QueryHandlers;
using Catalog.Application.Features.MovieCast.Queries;
using Catalog.Application.Features.MovieCast.Results;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.MovieCastTests
{
    public class GetMovieCastByIdQueryHandlerTests
    {
        private readonly Mock<IMovieCastRepository> _movieCastRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetMovieCastByIdQueryHandler>> _loggerMock;
        private readonly GetMovieCastByIdQueryHandler _handler;

        public GetMovieCastByIdQueryHandlerTests()
        {
            _movieCastRepositoryMock = new Mock<IMovieCastRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetMovieCastByIdQueryHandler>>();

            _handler = new GetMovieCastByIdQueryHandler(
                _movieCastRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMovieCast_WhenMovieCastExist()
        {
            // Arrange
            var query = new GetMovieCastByIdQuery("cast-1");
            var movieCast = new MovieCast
            {
                CastID = "cast-1",
                CastName = "Test Cast",
                Character = "Test Character",
                ImageURL = "test-image.jpg",
                MovieID = "test-movie-id",
                IsActive = true
            };

            _movieCastRepositoryMock.Setup(x => x.GetByIdAsync(query.CastID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(movieCast);

            var mappedResult = new GetMovieCastByIdQueryResult
            {
                CastID = "cast-1",
                CastName = "Test Cast",
                Character = "Test Character",
                ImageURL = "test-image.jpg",
                MovieID = "test-movie-id",
                IsActive = true
            };

            _mapperMock.Setup(x => x.Map<GetMovieCastByIdQueryResult>(movieCast))
                .Returns(mappedResult);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.CastID.Should().Be("cast-1");
            response.CastName.Should().Be("Test Cast");
            response.Character.Should().Be("Test Character");
            response.ImageURL.Should().Be("test-image.jpg");
            response.MovieID.Should().Be("test-movie-id");
            response.IsActive.Should().BeTrue();

            _movieCastRepositoryMock.Verify(x => x.GetByIdAsync(query.CastID, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetMovieCastByIdQueryResult>(movieCast), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenMovieCastNotFound()
        {
            // Arrange
            var query = new GetMovieCastByIdQuery("non-existent");

            _movieCastRepositoryMock.Setup(x => x.GetByIdAsync(query.CastID, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MovieCast)null!);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().BeNull();

            _movieCastRepositoryMock.Verify(x => x.GetByIdAsync(query.CastID, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetMovieCastByIdQueryResult>(It.IsAny<MovieCast>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var query = new GetMovieCastByIdQuery("cast-1");

            _movieCastRepositoryMock.Setup(x => x.GetByIdAsync(query.CastID, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>();

            _movieCastRepositoryMock.Verify(x => x.GetByIdAsync(query.CastID, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetMovieCastByIdQueryResult>(It.IsAny<MovieCast>()), Times.Never);
        }
    }
}