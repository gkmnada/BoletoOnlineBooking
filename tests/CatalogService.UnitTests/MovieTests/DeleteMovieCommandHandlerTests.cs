using AutoMapper;
using Boleto.Contracts.Events.MovieEvents;
using Catalog.Application.Features.Movie.Commands;
using Catalog.Application.Features.Movie.Handlers.CommandHandlers;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.MovieTests
{
    public class DeleteMovieCommandHandlerTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<ILogger<DeleteMovieCommandHandler>> _loggerMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;

        public DeleteMovieCommandHandlerTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _fileServiceMock = new Mock<IFileService>();
            _loggerMock = new Mock<ILogger<DeleteMovieCommandHandler>>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenMovieIsDeleted()
        {
            // Arrange
            var command = new DeleteMovieCommand("movie-123");

            var existingMovie = new Movie
            {
                MovieID = "movie-123",
                MovieName = "Old Movie",
                ImageURL = "old-image-url"
            };

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(command.MovieID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovie);

            _movieRepositoryMock.Setup(x => x.DeleteAsync(existingMovie)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovie.ImageURL))
                .ReturnsAsync(true);

            _mapperMock.Setup(x => x.Map<MovieDeleted>(existingMovie))
                .Returns(new MovieDeleted { MovieID = existingMovie.MovieID });

            var handler = new DeleteMovieCommandHandler(
                _movieRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                _publishEndpointMock.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Movie deleted successfully");
            response.Data.Should().Be(existingMovie.MovieID);

            _movieRepositoryMock.Verify(x => x.DeleteAsync(existingMovie), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<MovieDeleted>(), It.IsAny<CancellationToken>()), Times.Once);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(existingMovie.ImageURL), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenMovieDoesNotExist()
        {
            // Arrange
            var command = new DeleteMovieCommand("movie-123");

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(command.MovieID, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Movie)null!);

            var handler = new DeleteMovieCommandHandler(
                _movieRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                _publishEndpointMock.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Movie not found");

            _movieRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Movie>()), Times.Never);
            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<MovieDeleted>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenErrorOccurs()
        {
            // Arrange
            var command = new DeleteMovieCommand("movie-123");

            var existingMovie = new Movie
            {
                MovieID = "movie-123",
                MovieName = "Old Movie",
                ImageURL = "old-image-url"
            };

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(command.MovieID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovie);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var handler = new DeleteMovieCommandHandler(
                _movieRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                _publishEndpointMock.Object);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");

            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<MovieDeleted>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallFileDelete_WhenMovieIsDeletedSuccessfully()
        {
            // Arrange
            var command = new DeleteMovieCommand("movie-123");

            var existingMovie = new Movie
            {
                MovieID = "movie-123",
                MovieName = "Old Movie",
                ImageURL = "old-image-url"
            };

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(command.MovieID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovie);

            _movieRepositoryMock.Setup(x => x.DeleteAsync(existingMovie)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovie.ImageURL))
                .ReturnsAsync(true);

            var handler = new DeleteMovieCommandHandler(
                _movieRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                _publishEndpointMock.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            _fileServiceMock.Verify(x => x.DeleteFileAsync(existingMovie.ImageURL), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldPublishEvent_WhenMovieIsDeleted()
        {
            // Arrange
            var command = new DeleteMovieCommand("movie-123");

            var existingMovie = new Movie
            {
                MovieID = "movie-123",
                MovieName = "Old Movie",
                ImageURL = "old-image-url"
            };

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(command.MovieID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovie);

            _movieRepositoryMock.Setup(x => x.DeleteAsync(existingMovie)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovie.ImageURL))
                .ReturnsAsync(true);

            _mapperMock.Setup(x => x.Map<MovieDeleted>(existingMovie))
                .Returns(new MovieDeleted { MovieID = existingMovie.MovieID });

            var handler = new DeleteMovieCommandHandler(
                _movieRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                _publishEndpointMock.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<MovieDeleted>(), It.IsAny<CancellationToken>()), Times.Once);
            response.IsSuccess.Should().BeTrue();
        }
    }
}
