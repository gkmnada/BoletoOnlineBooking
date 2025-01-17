using AutoMapper;
using Catalog.Application.Features.MovieCast.Handlers.QueryHandlers;
using Catalog.Application.Features.MovieCast.Queries;
using Catalog.Application.Features.MovieCast.Results;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace CatalogService.UnitTests.MovieCastTests
{
    public class GetMovieCastsQueryHandlerTests
    {
        private readonly Mock<IMovieCastRepository> _movieCastRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetMovieCastsQueryHandler>> _loggerMock;
        private readonly GetMovieCastsQueryHandler _handler;

        public GetMovieCastsQueryHandlerTests()
        {
            _movieCastRepositoryMock = new Mock<IMovieCastRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetMovieCastsQueryHandler>>();

            _handler = new GetMovieCastsQueryHandler(
                _movieCastRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenMovieCastsExist()
        {
            // Arrange
            var query = new GetMovieCastsQuery("movie-1");
            var movieCasts = new List<MovieCast>
            {
                new MovieCast
                {
                    CastID = "cast-1",
                    CastName = "Cast 1",
                    Character = "Character 1",
                    ImageURL = "image1.jpg",
                    MovieID = "movie-1",
                    IsActive = true
                },
                new MovieCast
                {
                    CastID = "cast-2",
                    CastName = "Cast 2",
                    Character = "Character 2",
                    ImageURL = "image2.jpg",
                    MovieID = "movie-1",
                    IsActive = true
                }
            };

            _movieCastRepositoryMock.Setup(x => x.ListByFilterAsync(It.IsAny<Expression<Func<MovieCast, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(movieCasts);

            var mappedResults = new List<GetMovieCastsQueryResult>
            {
                new GetMovieCastsQueryResult
                {
                    CastID = "cast-1",
                    CastName = "Cast 1",
                    Character = "Character 1",
                    ImageURL = "image1.jpg",
                    MovieID = "movie-1",
                    IsActive = true
                },
                new GetMovieCastsQueryResult
                {
                    CastID = "cast-2",
                    CastName = "Cast 2",
                    Character = "Character 2",
                    ImageURL = "image2.jpg",
                    MovieID = "movie-1",
                    IsActive = true
                }
            };

            _mapperMock.Setup(x => x.Map<List<GetMovieCastsQueryResult>>(movieCasts))
                .Returns(mappedResults);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Count.Should().Be(2);
            response[0].CastName.Should().Be("Cast 1");
            response[0].Character.Should().Be("Character 1");
            response[1].CastName.Should().Be("Cast 2");
            response[1].Character.Should().Be("Character 2");

            _movieCastRepositoryMock.Verify(x => x.ListByFilterAsync(It.IsAny<Expression<Func<MovieCast, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<List<GetMovieCastsQueryResult>>(movieCasts), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoMovieCastsExist()
        {
            // Arrange
            var query = new GetMovieCastsQuery("movie-1");
            var movieCasts = new List<MovieCast>();

            _movieCastRepositoryMock.Setup(x => x.ListByFilterAsync(It.IsAny<Expression<Func<MovieCast, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(movieCasts);

            _mapperMock.Setup(x => x.Map<List<GetMovieCastsQueryResult>>(movieCasts))
                .Returns(new List<GetMovieCastsQueryResult>());

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Count.Should().Be(0);

            _movieCastRepositoryMock.Verify(x => x.ListByFilterAsync(It.IsAny<Expression<Func<MovieCast, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<List<GetMovieCastsQueryResult>>(movieCasts), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var query = new GetMovieCastsQuery("movie-1");

            _movieCastRepositoryMock.Setup(x => x.ListByFilterAsync(It.IsAny<Expression<Func<MovieCast, bool>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("An error occurred while fetching the movie casts"));

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing the request");

            _movieCastRepositoryMock.Verify(x => x.ListByFilterAsync(It.IsAny<Expression<Func<MovieCast, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<List<GetMovieCastsQueryResult>>(It.IsAny<List<MovieCast>>()), Times.Never);
        }
    }
}