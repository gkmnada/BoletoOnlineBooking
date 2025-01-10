using AutoMapper;
using Catalog.Application.Features.MovieDetail.Handlers.QueryHandler;
using Catalog.Application.Features.MovieDetail.Queries;
using Catalog.Application.Features.MovieDetail.Results;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace CatalogService.UnitTests.MovieDetailTests
{
    public class GetMovieDetailByMovieIdQueryHandlerTests
    {
        private readonly Mock<IMovieDetailRepository> _movieDetailRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetMovieDetailByMovieIdQueryHandler>> _loggerMock;
        private readonly GetMovieDetailByMovieIdQueryHandler _handler;

        public GetMovieDetailByMovieIdQueryHandlerTests()
        {
            _movieDetailRepositoryMock = new Mock<IMovieDetailRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetMovieDetailByMovieIdQueryHandler>>();

            _handler = new GetMovieDetailByMovieIdQueryHandler(
                _movieDetailRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMovieDetail_WhenMovieDetailExists()
        {
            // Arrange
            var query = new GetMovieDetailByMovieIdQuery("movie-123");
            var movieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                Description = "Test Description",
                MovieID = "movie-123",
                ImageURL = "image-url",
                VideoURL = "video-url",
                IsActive = true
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByFilterAsync(
                It.IsAny<Expression<Func<MovieDetail, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(movieDetail);

            var mappedResult = new GetMovieDetailByMovieIdQueryResult
            {
                DetailID = "detail-123",
                Description = "Test Description",
                MovieID = "movie-123",
                ImageURL = "image-url",
                VideoURL = "video-url",
                IsActive = true
            };

            _mapperMock.Setup(x => x.Map<GetMovieDetailByMovieIdQueryResult>(movieDetail))
                .Returns(mappedResult);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.DetailID.Should().Be("detail-123");
            response.Description.Should().Be("Test Description");
            response.MovieID.Should().Be("movie-123");
            response.ImageURL.Should().Be("image-url");
            response.VideoURL.Should().Be("video-url");
            response.IsActive.Should().BeTrue();

            _movieDetailRepositoryMock.Verify(x => x.GetByFilterAsync(
                It.Is<Expression<Func<MovieDetail, bool>>>(expr => expr.Compile()(movieDetail)),
                It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetMovieDetailByMovieIdQueryResult>(movieDetail), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenMovieDetailNotFound()
        {
            // Arrange
            var query = new GetMovieDetailByMovieIdQuery("non-existent-movie");

            _movieDetailRepositoryMock.Setup(x => x.GetByFilterAsync(
                It.IsAny<Expression<Func<MovieDetail, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync((MovieDetail)null!);

            _mapperMock.Setup(x => x.Map<GetMovieDetailByMovieIdQueryResult>(null))
                .Returns((GetMovieDetailByMovieIdQueryResult)null!);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().BeNull();

            _movieDetailRepositoryMock.Verify(x => x.GetByFilterAsync(
                It.IsAny<Expression<Func<MovieDetail, bool>>>(),
                It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetMovieDetailByMovieIdQueryResult>(null), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var query = new GetMovieDetailByMovieIdQuery("movie-123");

            _movieDetailRepositoryMock.Setup(x => x.GetByFilterAsync(
                It.IsAny<Expression<Func<MovieDetail, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing the request");

            _movieDetailRepositoryMock.Verify(x => x.GetByFilterAsync(
                It.IsAny<Expression<Func<MovieDetail, bool>>>(),
                It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetMovieDetailByMovieIdQueryResult>(It.IsAny<MovieDetail>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldVerifyCorrectFilterExpression()
        {
            // Arrange
            var movieId = "movie-123";
            var query = new GetMovieDetailByMovieIdQuery(movieId);

            var movieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                MovieID = movieId
            };

            Expression<Func<MovieDetail, bool>> capturedFilter = null!;

            _movieDetailRepositoryMock.Setup(x => x.GetByFilterAsync(
                It.IsAny<Expression<Func<MovieDetail, bool>>>(),
                It.IsAny<CancellationToken>()))
                .Callback<Expression<Func<MovieDetail, bool>>, CancellationToken>((filter, _) => capturedFilter = filter)
                .ReturnsAsync(movieDetail);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            capturedFilter.Should().NotBeNull();
            var compiledFilter = capturedFilter.Compile();

            var matchingDetail = new MovieDetail { MovieID = movieId };
            compiledFilter(matchingDetail).Should().BeTrue();

            var nonMatchingDetail = new MovieDetail { MovieID = "different-id" };
            compiledFilter(nonMatchingDetail).Should().BeFalse();
        }
    }
}