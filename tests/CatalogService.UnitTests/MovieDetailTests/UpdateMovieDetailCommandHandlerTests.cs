using AutoMapper;
using Catalog.Application.Common.Base;
using Catalog.Application.Features.MovieDetail.Commands;
using Catalog.Application.Features.MovieDetail.Handlers.CommandHandlers;
using Catalog.Application.Features.MovieDetail.Validators;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.MovieDetailTests
{
    public class UpdateMovieDetailCommandHandlerTests
    {
        private readonly Mock<IMovieDetailRepository> _movieDetailRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<ILogger<UpdateMovieDetailCommandHandler>> _loggerMock;
        private readonly UpdateMovieDetailCommandHandler _handler;

        public UpdateMovieDetailCommandHandlerTests()
        {
            _movieDetailRepositoryMock = new Mock<IMovieDetailRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _fileServiceMock = new Mock<IFileService>();
            _loggerMock = new Mock<ILogger<UpdateMovieDetailCommandHandler>>();

            _handler = new UpdateMovieDetailCommandHandler(
                _movieDetailRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                new UpdateMovieDetailValidator());
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenMovieDetailIsUpdated()
        {
            // Arrange
            var imageFileMock = new Mock<IFormFile>();
            imageFileMock.Setup(x => x.FileName).Returns("test-image.png");
            imageFileMock.Setup(x => x.Length).Returns(1024);
            imageFileMock.Setup(x => x.ContentType).Returns("image/png");
            imageFileMock.Setup(x => x.OpenReadStream()).Returns(new MemoryStream(new byte[1024]));

            var videoFileMock = new Mock<IFormFile>();
            videoFileMock.Setup(x => x.FileName).Returns("test-video.mp4");
            videoFileMock.Setup(x => x.Length).Returns(1024);
            videoFileMock.Setup(x => x.ContentType).Returns("video/mp4");
            videoFileMock.Setup(x => x.OpenReadStream()).Returns(new MemoryStream(new byte[1024]));

            var command = new UpdateMovieDetailCommand
            {
                DetailID = "detail-123",
                Description = "Updated Movie Description",
                MovieID = "movie-123",
                ImageURL = imageFileMock.Object,
                VideoURL = videoFileMock.Object,
                IsActive = true
            };

            var existingMovieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                Description = "Old Description",
                MovieID = "movie-123",
                ImageURL = "old-image-url",
                VideoURL = "old-video-url"
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(command.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieDetail);

            var validator = new UpdateMovieDetailValidator();
            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeTrue("command should be valid");

            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.ImageURL))
                .ReturnsAsync(true);
            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.VideoURL))
                .ReturnsAsync(true);

            _fileServiceMock.Setup(x => x.UploadImageAsync(command.ImageURL))
                .ReturnsAsync("new-uploaded-image-url");
            _fileServiceMock.Setup(x => x.UploadVideoAsync(command.VideoURL))
                .ReturnsAsync("new-uploaded-video-url");

            _mapperMock.Setup(x => x.Map(command, existingMovieDetail)).Returns(existingMovieDetail);

            _movieDetailRepositoryMock.Setup(x => x.UpdateAsync(existingMovieDetail)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Movie detail updated successfully");

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _movieDetailRepositoryMock.Verify(x => x.UpdateAsync(existingMovieDetail), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _fileServiceMock.Verify(x => x.DeleteFileAsync("old-image-url"), Times.Once);
            _fileServiceMock.Verify(x => x.DeleteFileAsync("old-video-url"), Times.Once);
            _fileServiceMock.Verify(x => x.UploadImageAsync(command.ImageURL), Times.Once);
            _fileServiceMock.Verify(x => x.UploadVideoAsync(command.VideoURL), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenMovieDetailDoesNotExist()
        {
            // Arrange
            var command = new UpdateMovieDetailCommand
            {
                DetailID = "non-existent-detail",
                Description = "Does not matter",
                MovieID = "movie-123",
                ImageURL = new Mock<IFormFile>().Object,
                VideoURL = new Mock<IFormFile>().Object
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(command.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MovieDetail)null!);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Movie detail not found");

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _movieDetailRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MovieDetail>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Never);
            _fileServiceMock.Verify(x => x.UploadImageAsync(It.IsAny<IFormFile>()), Times.Never);
            _fileServiceMock.Verify(x => x.UploadVideoAsync(It.IsAny<IFormFile>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new UpdateMovieDetailCommand
            {
                DetailID = "detail-123",
                Description = "", // Invalid: empty description
                MovieID = "",    // Invalid: empty movie ID
                ImageURL = null!,
                VideoURL = null!
            };

            var existingMovieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                Description = "Old Description",
                MovieID = "movie-123",
                ImageURL = "old-image-url",
                VideoURL = "old-video-url"
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(command.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieDetail);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");
            response.Errors.Should().NotBeNullOrEmpty();

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _movieDetailRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MovieDetail>()), Times.Never);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Never);
            _fileServiceMock.Verify(x => x.UploadImageAsync(It.IsAny<IFormFile>()), Times.Never);
            _fileServiceMock.Verify(x => x.UploadVideoAsync(It.IsAny<IFormFile>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenFilesAreMissing()
        {
            // Arrange
            var command = new UpdateMovieDetailCommand
            {
                DetailID = "detail-123",
                Description = "Updated Description",
                MovieID = "movie-123",
                ImageURL = null!,
                VideoURL = null!
            };

            var existingMovieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                Description = "Old Description",
                MovieID = "movie-123",
                ImageURL = "old-image-url",
                VideoURL = "old-video-url"
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(command.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieDetail);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _movieDetailRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MovieDetail>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Never);
            _fileServiceMock.Verify(x => x.UploadImageAsync(It.IsAny<IFormFile>()), Times.Never);
            _fileServiceMock.Verify(x => x.UploadVideoAsync(It.IsAny<IFormFile>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenImageUploadFails()
        {
            // Arrange
            var imageFileMock = new Mock<IFormFile>();
            imageFileMock.Setup(x => x.FileName).Returns("test-image.png");
            imageFileMock.Setup(x => x.Length).Returns(1024);

            var videoFileMock = new Mock<IFormFile>();
            videoFileMock.Setup(x => x.FileName).Returns("test-video.mp4");
            videoFileMock.Setup(x => x.Length).Returns(1024);

            var command = new UpdateMovieDetailCommand
            {
                DetailID = "detail-123",
                Description = "Updated Description",
                MovieID = "movie-123",
                ImageURL = imageFileMock.Object,
                VideoURL = videoFileMock.Object
            };

            var existingMovieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                Description = "Old Description",
                MovieID = "movie-123",
                ImageURL = "old-image-url",
                VideoURL = "old-video-url"
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(command.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieDetail);

            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.ImageURL))
                .ReturnsAsync(true);
            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.VideoURL))
                .ReturnsAsync(true);

            _fileServiceMock.Setup(x => x.UploadImageAsync(imageFileMock.Object))
                .ThrowsAsync(new Exception("Image upload failed"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing the request");

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _movieDetailRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MovieDetail>()), Times.Never);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(existingMovieDetail.ImageURL), Times.Once);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(existingMovieDetail.VideoURL), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenVideoUploadFails()
        {
            // Arrange
            var imageFileMock = new Mock<IFormFile>();
            imageFileMock.Setup(x => x.FileName).Returns("test-image.png");
            imageFileMock.Setup(x => x.Length).Returns(1024);

            var videoFileMock = new Mock<IFormFile>();
            videoFileMock.Setup(x => x.FileName).Returns("test-video.mp4");
            videoFileMock.Setup(x => x.Length).Returns(1024);

            var command = new UpdateMovieDetailCommand
            {
                DetailID = "detail-123",
                Description = "Updated Description",
                MovieID = "movie-123",
                ImageURL = imageFileMock.Object,
                VideoURL = videoFileMock.Object
            };

            var existingMovieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                Description = "Old Description",
                MovieID = "movie-123",
                ImageURL = "old-image-url",
                VideoURL = "old-video-url"
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(command.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieDetail);

            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.ImageURL))
                .ReturnsAsync(true);
            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.VideoURL))
                .ReturnsAsync(true);

            _fileServiceMock.Setup(x => x.UploadImageAsync(imageFileMock.Object))
                .ReturnsAsync("new-uploaded-image-url");
            _fileServiceMock.Setup(x => x.UploadVideoAsync(videoFileMock.Object))
                .ThrowsAsync(new Exception("Video upload failed"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing the request");

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _movieDetailRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MovieDetail>()), Times.Never);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(existingMovieDetail.ImageURL), Times.Once);
            _fileServiceMock.Verify(x => x.DeleteFileAsync(existingMovieDetail.VideoURL), Times.Once);
            _fileServiceMock.Verify(x => x.UploadImageAsync(imageFileMock.Object), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRepositorySaveFails()
        {
            // Arrange
            var imageFileMock = new Mock<IFormFile>();
            imageFileMock.Setup(x => x.FileName).Returns("test-image.png");
            imageFileMock.Setup(x => x.Length).Returns(1024);

            var videoFileMock = new Mock<IFormFile>();
            videoFileMock.Setup(x => x.FileName).Returns("test-video.mp4");
            videoFileMock.Setup(x => x.Length).Returns(1024);

            var command = new UpdateMovieDetailCommand
            {
                DetailID = "detail-123",
                Description = "Updated Description",
                MovieID = "movie-123",
                ImageURL = imageFileMock.Object,
                VideoURL = videoFileMock.Object
            };

            var existingMovieDetail = new MovieDetail
            {
                DetailID = "detail-123",
                Description = "Old Description",
                MovieID = "movie-123",
                ImageURL = "old-image-url",
                VideoURL = "old-video-url"
            };

            _movieDetailRepositoryMock.Setup(x => x.GetByIdAsync(command.DetailID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieDetail);

            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.ImageURL))
                .ReturnsAsync(true);
            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovieDetail.VideoURL))
                .ReturnsAsync(true);

            _fileServiceMock.Setup(x => x.UploadImageAsync(imageFileMock.Object))
                .ReturnsAsync("new-uploaded-image-url");
            _fileServiceMock.Setup(x => x.UploadVideoAsync(videoFileMock.Object))
                .ReturnsAsync("new-uploaded-video-url");

            _mapperMock.Setup(x => x.Map(command, existingMovieDetail)).Returns(existingMovieDetail);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database save failed"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing the request");

            _movieDetailRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _movieDetailRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MovieDetail>()), Times.Once);
            _fileServiceMock.Verify(x => x.DeleteFileAsync("old-image-url"), Times.Once);
            _fileServiceMock.Verify(x => x.DeleteFileAsync("old-video-url"), Times.Once);
            _fileServiceMock.Verify(x => x.UploadImageAsync(imageFileMock.Object), Times.Once);
            _fileServiceMock.Verify(x => x.UploadVideoAsync(videoFileMock.Object), Times.Once);
        }
    }
}