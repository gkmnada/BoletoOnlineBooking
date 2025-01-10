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
    public class GetMoviesQueryHandlerTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetMoviesQueryHandler>> _loggerMock;
        private readonly GetMoviesQueryHandler _handler;

        public GetMoviesQueryHandlerTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetMoviesQueryHandler>>();

            _handler = new GetMoviesQueryHandler(
                _movieRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMovies_WhenMoviesExist()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { MovieID = "1", MovieName = "Movie 1" },
                new Movie { MovieID = "2", MovieName = "Movie 2" }
            };

            _movieRepositoryMock.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(movies);

            var mappedResults = new List<GetMoviesQueryResult>
            {
                new GetMoviesQueryResult { MovieID = "1", MovieName = "Movie 1" },
                new GetMoviesQueryResult { MovieID = "2", MovieName = "Movie 2" }
            };

            _mapperMock.Setup(x => x.Map<List<GetMoviesQueryResult>>(movies))
                .Returns(mappedResults);

            // Act
            var response = await _handler.Handle(new GetMoviesQuery(), CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Count.Should().Be(2);
            response[0].MovieName.Should().Be("Movie 1");
            response[1].MovieName.Should().Be("Movie 2");

            _movieRepositoryMock.Verify(x => x.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<List<GetMoviesQueryResult>>(movies), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoMoviesExist()
        {
            // Arrange
            var movies = new List<Movie>(); // Empty

            _movieRepositoryMock.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(movies);

            _mapperMock.Setup(x => x.Map<List<GetMoviesQueryResult>>(movies))
                .Returns(new List<GetMoviesQueryResult>());

            // Act
            var response = await _handler.Handle(new GetMoviesQuery(), CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Count.Should().Be(0);

            _movieRepositoryMock.Verify(x => x.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            _movieRepositoryMock.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () => await _handler.Handle(new GetMoviesQuery(), CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");

            _movieRepositoryMock.Verify(x => x.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
