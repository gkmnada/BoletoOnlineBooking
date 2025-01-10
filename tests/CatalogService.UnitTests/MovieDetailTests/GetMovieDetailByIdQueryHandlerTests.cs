using AutoMapper;
using Catalog.Application.Features.MovieDetail.Handlers.QueryHandler;
using Catalog.Application.Features.MovieDetail.Queries;
using Catalog.Application.Features.MovieDetail.Results;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.MovieDetailTests
{
    public class GetMovieDetailByIdQueryHandlerTests
    {
        private readonly Mock<IMovieDetailRepository> _movieDetailRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetMovieDetailByIdQueryHandler>> _loggerMock;
        private readonly GetMovieDetailByIdQueryHandler _handler;

        public GetMovieDetailByIdQueryHandlerTests()
        {
            _movieDetailRepositoryMock = new Mock<IMovieDetailRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetMovieDetailByIdQueryHandler>>();

            _handler = new GetMovieDetailByIdQueryHandler(
                _movieDetailRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMovieDetail_WhenMovieDetailExists()
        {
            // Arrange
            var query = new GetMovieDetailByIdQuery("detail-123");
            var movieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                Description = "Test Description",
                MovieID = "movie-123",
                ImageURL = "image-url",
                VideoURL = "video-url",
                IsActive = true
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(query.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(movieDetail);

            var mappedResult = new GetMovieDetailByIdQueryResult
            {
                DetailID = "detail-123",
                Description = "Test Description",
                MovieID = "movie-123",
                ImageURL = "image-url",
                VideoURL = "video-url",
                IsActive = true
            };

            _mapperMock.Setup(x => x.Map<GetMovieDetailByIdQueryResult>(movieDetail))
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

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(query.DetailID, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetMovieDetailByIdQueryResult>(movieDetail), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenMovieDetailNotFound()
        {
            // Arrange
            var query = new GetMovieDetailByIdQuery("non-existent");

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(query.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MovieDetail)null!);

            _mapperMock.Setup(x => x.Map<GetMovieDetailByIdQueryResult>(null))
                .Returns((GetMovieDetailByIdQueryResult)null!);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().BeNull();

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(query.DetailID, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetMovieDetailByIdQueryResult>(null), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var query = new GetMovieDetailByIdQuery("detail-123");

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(query.DetailID, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing the request");

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(query.DetailID, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<GetMovieDetailByIdQueryResult>(It.IsAny<MovieDetail>()), Times.Never);
        }
    }
}