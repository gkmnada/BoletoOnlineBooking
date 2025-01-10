using Catalog.Application.Features.MovieDetail.Commands;
using Catalog.Application.Features.MovieDetail.Handlers.CommandHandlers;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.MovieDetailTests
{
    public class DeleteMovieDetailCommandHandlerTests
    {
        private readonly Mock<IMovieDetailRepository> _movieDetailRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<ILogger<DeleteMovieDetailCommandHandler>> _loggerMock;
        private readonly DeleteMovieDetailCommandHandler _handler;

        public DeleteMovieDetailCommandHandlerTests()
        {
            _movieDetailRepositoryMock = new Mock<IMovieDetailRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _fileServiceMock = new Mock<IFileService>();
            _loggerMock = new Mock<ILogger<DeleteMovieDetailCommandHandler>>();

            _handler = new DeleteMovieDetailCommandHandler(
                _movieDetailRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenMovieDetailIsDeleted()
        {
            // Arrange
            var command = new DeleteMovieDetailCommand("detail-123");

            var existingMovieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                Description = "Test Description",
                MovieID = "movie-123",
                ImageURL = "image-url",
                VideoURL = "video-url"
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(command.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieDetail);

            _movieDetailRepositoryMock.Setup(x => x.DeleteAsync(existingMovieDetail)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.ImageURL))
                .ReturnsAsync(true);
            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.VideoURL))
                .ReturnsAsync(true);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Movie detail deleted successfully");

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _movieDetailRepositoryMock.Verify(x => x.DeleteAsync(existingMovieDetail), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(existingMovieDetail.ImageURL), Times.Once);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(existingMovieDetail.VideoURL), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenMovieDetailDoesNotExist()
        {
            // Arrange
            var command = new DeleteMovieDetailCommand("non-existent-detail");

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(command.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MovieDetail)null!);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Movie detail not found");

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _movieDetailRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<MovieDetail>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var command = new DeleteMovieDetailCommand("detail-123");

            var existingMovieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                Description = "Test Description",
                MovieID = "movie-123",
                ImageURL = "image-url",
                VideoURL = "video-url"
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(command.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieDetail);

            _movieDetailRepositoryMock.Setup(x => x.DeleteAsync(existingMovieDetail))
                .Throws(new Exception("An error occurred while deleting the movie detail"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _movieDetailRepositoryMock.Verify(x => x.DeleteAsync(existingMovieDetail), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldDeleteFiles_WhenMovieDetailIsDeletedSuccessfully()
        {
            // Arrange
            var command = new DeleteMovieDetailCommand("detail-123");

            var existingMovieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                Description = "Test Description",
                MovieID = "movie-123",
                ImageURL = "image-url",
                VideoURL = "video-url"
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(command.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieDetail);

            _movieDetailRepositoryMock.Setup(x => x.DeleteAsync(existingMovieDetail)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.ImageURL))
                .ReturnsAsync(true);
            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.VideoURL))
                .ReturnsAsync(true);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(existingMovieDetail.ImageURL), Times.Once);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(existingMovieDetail.VideoURL), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenFileDeleteFails()
        {
            // Arrange
            var command = new DeleteMovieDetailCommand("detail-123");

            var existingMovieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                Description = "Test Description",
                MovieID = "movie-123",
                ImageURL = "image-url",
                VideoURL = "video-url"
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(command.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieDetail);

            _movieDetailRepositoryMock.Setup(x => x.DeleteAsync(existingMovieDetail)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.ImageURL))
                .ThrowsAsync(new Exception("File delete failed"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _movieDetailRepositoryMock.Verify(x => x.DeleteAsync(existingMovieDetail), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(existingMovieDetail.ImageURL), Times.Once);
        }
    }
}