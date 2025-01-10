using AutoMapper;
using Boleto.Contracts.Events.MovieEvents;
using Catalog.Application.Features.Movie.Commands;
using Catalog.Application.Features.Movie.Handlers.CommandHandlers;
using Catalog.Application.Features.Movie.Validators;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.MovieTests
{
    public class CreateMovieCommandHandlerTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<ILogger<CreateMovieCommandHandler>> _loggerMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly CreateMovieCommandHandler _handler;

        public CreateMovieCommandHandlerTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _fileServiceMock = new Mock<IFileService>();
            _loggerMock = new Mock<ILogger<CreateMovieCommandHandler>>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();

            _handler = new CreateMovieCommandHandler(
                _movieRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                new CreateMovieValidator(),
                _publishEndpointMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenMovieIsCreated()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(x => x.FileName).Returns("test-image.png");
            fileMock.Setup(x => x.Length).Returns(1024);
            fileMock.Setup(x => x.ContentType).Returns("image/png");
            fileMock.Setup(x => x.OpenReadStream()).Returns(new MemoryStream(new byte[1024]));

            var command = new CreateMovieCommand
            {
                MovieName = "Test Movie",
                Genre = new List<string> { "Action" },
                Language = new List<string> { "English" },
                Duration = "148 mins",
                ReleaseDate = "2024-12-6",
                Rating = 5,
                AudienceScore = 5,
                CategoryID = "84f0b4ed-37eb-49e0-81e5-e643be786e04",
                SlugURL = "test-movie",
                ImageURL = fileMock.Object
            };

            _fileServiceMock.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("upload-image-url");

            var validator = new CreateMovieValidator();
            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeTrue("command should be valid");

            var movie = new Movie
            {
                MovieID = "movie-123",
                MovieName = command.MovieName,
                Genre = command.Genre,
                Language = command.Language,
                Duration = command.Duration,
                ReleaseDate = command.ReleaseDate,
                Rating = command.Rating,
                AudienceScore = command.AudienceScore,
                CategoryID = command.CategoryID,
                ImageURL = "uploaded-image-url"
            };

            _mapperMock.Setup(x => x.Map<Movie>(command)).Returns(movie);

            _movieRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Movie>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Movie created successfully");
            response.Data.Should().Be(movie.MovieID);

            _movieRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Movie>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _fileServiceMock.Verify(x => x.UploadImageAsync(It.IsAny<IFormFile>()), Times.Once);
            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<MovieCreated>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreateMovieCommand
            {
                MovieName = "",
                Genre = new List<string> { "Action" },
                Language = new List<string> { "English" },
                Duration = "148 mins",
                ReleaseDate = "2024-12-6",
                Rating = 5,
                AudienceScore = 5,
                CategoryID = "84f0b4ed-37eb-49e0-81e5-e643be786e04",
                SlugURL = "test-movie",
                ImageURL = null!
            };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");
            response.Errors.Should().NotBeNullOrEmpty();

            _movieRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Movie>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _fileServiceMock.Verify(x => x.UploadImageAsync(It.IsAny<IFormFile>()), Times.Never);
            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<MovieCreated>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenImageUploadFails()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(x => x.FileName).Returns("test-image.png");
            fileMock.Setup(x => x.Length).Returns(1024);
            fileMock.Setup(x => x.ContentType).Returns("image/png");
            fileMock.Setup(x => x.OpenReadStream()).Returns(new MemoryStream(new byte[1024]));

            var command = new CreateMovieCommand
            {
                MovieName = "Test Movie",
                Genre = new List<string> { "Action" },
                Language = new List<string> { "English" },
                Duration = "148 mins",
                ReleaseDate = "2024-12-6",
                Rating = 5,
                AudienceScore = 5,
                CategoryID = "84f0b4ed-37eb-49e0-81e5-e643be786e04",
                SlugURL = "test-movie",
                ImageURL = fileMock.Object
            };

            _fileServiceMock.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>()))
                .ThrowsAsync(new Exception("Image upload failed"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRepositorySaveFails()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(x => x.FileName).Returns("test-image.png");
            fileMock.Setup(x => x.Length).Returns(1024);
            fileMock.Setup(x => x.ContentType).Returns("image/png");
            fileMock.Setup(x => x.OpenReadStream()).Returns(new MemoryStream(new byte[1024]));

            var command = new CreateMovieCommand
            {
                MovieName = "Test Movie",
                Genre = new List<string> { "Action" },
                Language = new List<string> { "English" },
                Duration = "148 mins",
                ReleaseDate = "2024-12-6",
                Rating = 5,
                AudienceScore = 5,
                CategoryID = "84f0b4ed-37eb-49e0-81e5-e643be786e04",
                SlugURL = "test-movie",
                ImageURL = fileMock.Object
            };

            _fileServiceMock.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("uploaded-image-url");

            _mapperMock.Setup(x => x.Map<Movie>(command)).Returns(new Movie
            {
                MovieID = "movie-123",
                MovieName = "Test Movie",
                Genre = command.Genre,
                Language = command.Language,
                Duration = command.Duration,
                ReleaseDate = command.ReleaseDate,
                Rating = command.Rating,
                AudienceScore = command.AudienceScore,
                CategoryID = command.CategoryID,
                ImageURL = "uploaded-image-url"
            });

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database save failed"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");
        }

        [Fact]
        public async Task Handle_ShouldPublishEvent_WhenMovieIsCreated()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(x => x.FileName).Returns("test-image.png");
            fileMock.Setup(x => x.Length).Returns(1024);
            fileMock.Setup(x => x.ContentType).Returns("image/png");
            fileMock.Setup(x => x.OpenReadStream()).Returns(new MemoryStream(new byte[1024]));

            var command = new CreateMovieCommand
            {
                MovieName = "Test Movie",
                Genre = new List<string> { "Action" },
                Language = new List<string> { "English" },
                Duration = "148 mins",
                ReleaseDate = "2024-12-6",
                Rating = 5,
                AudienceScore = 5,
                CategoryID = "84f0b4ed-37eb-49e0-81e5-e643be786e04",
                SlugURL = "test-movie",
                ImageURL = fileMock.Object
            };

            _fileServiceMock.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("uploaded-image-url");

            _mapperMock.Setup(x => x.Map<Movie>(command)).Returns(new Movie
            {
                MovieID = "movie-123",
                MovieName = "Test Movie",
                Genre = command.Genre,
                Language = command.Language,
                Duration = command.Duration,
                ReleaseDate = command.ReleaseDate,
                Rating = command.Rating,
                AudienceScore = command.AudienceScore,
                CategoryID = command.CategoryID,
                ImageURL = "uploaded-image-url"
            });

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<MovieCreated>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
