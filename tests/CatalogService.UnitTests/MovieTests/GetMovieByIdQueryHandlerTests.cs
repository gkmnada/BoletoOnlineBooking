using AutoMapper;
using Catalog.Application.Features.Movie.Handlers.QueryHandlers;
using Catalog.Application.Features.Movie.Queries;
using Catalog.Application.Features.Movie.Results;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.MovieTests
{
    public class GetMovieByIdQueryHandlerTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetMovieByIdQueryHandler>> _loggerMock;

        public GetMovieByIdQueryHandlerTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetMovieByIdQueryHandler>>();
        }

        [Fact]
        public async Task Handle_ShouldReturnMovie_WhenMovieExist()
        {
            // Arrange
            var query = new GetMovieByIdQuery("1");
            var movie = new Movie { MovieID = "1", MovieName = "Test Movie" };

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(query.MovieID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(movie);

            var mapper = new GetMovieByIdQueryResult { MovieID = "1", MovieName = "Test Movie" };

            _mapperMock.Setup(x => x.Map<GetMovieByIdQueryResult>(movie))
                .Returns(mapper);

            var handler = new GetMovieByIdQueryHandler(_movieRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.MovieID.Should().Be("1");
            response.MovieName.Should().Be("Test Movie");

            _movieRepositoryMock.Verify(x => x.GetByIdAsync(query.MovieID, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetMovieByIdQueryResult>(movie), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenMovieNotFound()
        {
            // Arrange
            var query = new GetMovieByIdQuery("non-existent");

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(query.MovieID, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Movie)null!);

            _mapperMock.Setup(x => x.Map<GetMovieByIdQueryResult>(null))
                .Returns((GetMovieByIdQueryResult)null!);

            var handler = new GetMovieByIdQueryHandler(_movieRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().BeNull();

            _movieRepositoryMock.Verify(x => x.GetByIdAsync(query.MovieID, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetMovieByIdQueryResult>(null), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var query = new GetMovieByIdQuery("1");

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(query.MovieID, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var handler = new GetMovieByIdQueryHandler(_movieRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");

            _movieRepositoryMock.Verify(x => x.GetByIdAsync(query.MovieID, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetMovieByIdQueryResult>(It.IsAny<Movie>()), Times.Never);
        }
    }
}
