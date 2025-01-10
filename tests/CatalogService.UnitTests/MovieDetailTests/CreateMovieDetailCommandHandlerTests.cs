using AutoMapper;
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
    public class CreateMovieDetailCommandHandlerTests
    {
        private readonly Mock<IMovieDetailRepository> _movieDetailRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<ILogger<CreateMovieDetailCommandHandler>> _loggerMock;
        private readonly CreateMovieDetailCommandHandler _handler;

        public CreateMovieDetailCommandHandlerTests()
        {
            _movieDetailRepositoryMock = new Mock<IMovieDetailRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _fileServiceMock = new Mock<IFileService>();
            _loggerMock = new Mock<ILogger<CreateMovieDetailCommandHandler>>();

            _handler = new CreateMovieDetailCommandHandler(
                _movieDetailRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                new CreateMovieDetailValidator());
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenMovieDetailIsCreated()
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

            var command = new CreateMovieDetailCommand
            {
                Description = "Test Movie Description",
                MovieID = "movie-123",
                ImageURL = imageFileMock.Object,
                VideoURL = videoFileMock.Object
            };

            _fileServiceMock.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("uploaded-image-url");
            _fileServiceMock.Setup(x => x.UploadVideoAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("uploaded-video-url");

            var validator = new CreateMovieDetailValidator();
            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeTrue("command should be valid");

            var movieDetail = new MovieDetail
            {
                Description = command.Description,
                MovieID = command.MovieID,
                ImageURL = "uploaded-image-url",
                VideoURL = "uploaded-video-url"
            };

            _mapperMock.Setup(x => x.Map<MovieDetail>(command)).Returns(movieDetail);

            _movieDetailRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<MovieDetail>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Movie detail created successfully");

            _movieDetailRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<MovieDetail>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _fileServiceMock.Verify(x => x.UploadImageAsync(It.IsAny<IFormFile>()), Times.Once);
            _fileServiceMock.Verify(x => x.UploadVideoAsync(It.IsAny<IFormFile>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreateMovieDetailCommand
            {
                Description = "",
                MovieID = "",
                ImageURL = null!,
                VideoURL = null!
            };

            var validator = new CreateMovieDetailValidator();
            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeFalse("command should be invalid");

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");
            response.Errors.Should().NotBeNullOrEmpty();

            _movieDetailRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<MovieDetail>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
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
            imageFileMock.Setup(x => x.ContentType).Returns("image/png");
            imageFileMock.Setup(x => x.OpenReadStream()).Returns(new MemoryStream(new byte[1024]));

            var videoFileMock = new Mock<IFormFile>();
            videoFileMock.Setup(x => x.FileName).Returns("test-video.mp4");
            videoFileMock.Setup(x => x.Length).Returns(1024);
            videoFileMock.Setup(x => x.ContentType).Returns("video/mp4");
            videoFileMock.Setup(x => x.OpenReadStream()).Returns(new MemoryStream(new byte[1024]));

            var command = new CreateMovieDetailCommand
            {
                Description = "Test Movie Description",
                MovieID = "movie-123",
                ImageURL = imageFileMock.Object,
                VideoURL = videoFileMock.Object
            };

            _fileServiceMock.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>()))
                .ThrowsAsync(new Exception("Image upload failed"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenVideoUploadFails()
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

            var command = new CreateMovieDetailCommand
            {
                Description = "Test Movie Description",
                MovieID = "movie-123",
                ImageURL = imageFileMock.Object,
                VideoURL = videoFileMock.Object
            };

            _fileServiceMock.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("uploaded-image-url");
            _fileServiceMock.Setup(x => x.UploadVideoAsync(It.IsAny<IFormFile>()))
                .ThrowsAsync(new Exception("Video upload failed"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRepositorySaveFails()
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

            var command = new CreateMovieDetailCommand
            {
                Description = "Test Movie Description",
                MovieID = "movie-123",
                ImageURL = imageFileMock.Object,
                VideoURL = videoFileMock.Object
            };

            _fileServiceMock.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("uploaded-image-url");
            _fileServiceMock.Setup(x => x.UploadVideoAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("uploaded-video-url");

            _mapperMock.Setup(x => x.Map<MovieDetail>(command)).Returns(new MovieDetail
            {
                Description = command.Description,
                MovieID = command.MovieID,
                ImageURL = "uploaded-image-url",
                VideoURL = "uploaded-video-url"
            });

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database save failed"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenFilesAreMissing()
        {
            // Arrange
            var command = new CreateMovieDetailCommand
            {
                Description = "Test Movie Description",
                MovieID = "movie-123",
                ImageURL = null!,
                VideoURL = null!
            };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");

            _movieDetailRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<MovieDetail>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fileServiceMock.Verify(x => x.UploadImageAsync(It.IsAny<IFormFile>()), Times.Never);
            _fileServiceMock.Verify(x => x.UploadVideoAsync(It.IsAny<IFormFile>()), Times.Never);
        }
    }
}