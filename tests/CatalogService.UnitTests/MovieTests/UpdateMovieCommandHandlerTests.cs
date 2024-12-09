﻿using AutoMapper;
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
    public class UpdateMovieCommandHandlerTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<ILogger<UpdateMovieCommandHandler>> _loggerMock;
        private readonly Mock<UpdateMovieValidator> _validatorMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;

        public UpdateMovieCommandHandlerTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _fileServiceMock = new Mock<IFileService>();
            _loggerMock = new Mock<ILogger<UpdateMovieCommandHandler>>();
            _validatorMock = new Mock<UpdateMovieValidator>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenMovieIsUpdated()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(x => x.FileName).Returns("test-image.png");
            fileMock.Setup(x => x.Length).Returns(1024);
            fileMock.Setup(x => x.ContentType).Returns("image/png");
            fileMock.Setup(x => x.OpenReadStream()).Returns(new MemoryStream(new byte[1024]));

            var command = new UpdateMovieCommand
            {
                MovieID = "movie-123",
                MovieName = "Updated Movie",
                Genre = new List<string> { "Adventure" },
                Language = new List<string> { "French" },
                Duration = "120 mins",
                ReleaseDate = "2025-01-01",
                Rating = 4,
                AudienceScore = 4,
                CategoryID = "84f0b4ed-37eb-49e0-81e5-e643be786e04",
                SlugURL = "updated-movie",
                ImageURL = fileMock.Object
            };

            var existingMovie = new Movie
            {
                MovieID = "movie-123",
                MovieName = "Old Movie",
                Genre = new List<string> { "Action" },
                Language = new List<string> { "English" },
                Duration = "100 mins",
                ReleaseDate = "2024-12-01",
                Rating = 3,
                AudienceScore = 3,
                CategoryID = "84f0b4ed-37eb-49e0-81e5-e643be786e04",
                ImageURL = "old-image-url"
            };

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(command.MovieID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovie);

            var validator = new UpdateMovieValidator();
            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeTrue("command should be valid");

            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovie.ImageURL))
                .ReturnsAsync(true);

            _fileServiceMock.Setup(x => x.UploadImageAsync(command.ImageURL))
                .ReturnsAsync("new-uploaded-image-url");

            _mapperMock.Setup(x => x.Map(command, existingMovie)).Returns(existingMovie);

            _movieRepositoryMock.Setup(x => x.UpdateAsync(existingMovie)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new UpdateMovieCommandHandler(
                _movieRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                validator,
                _publishEndpointMock.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Movie updated successfully");
            response.Data.Should().Be(existingMovie.MovieID);

            _movieRepositoryMock.Verify(x => x.UpdateAsync(existingMovie), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<MovieUpdated>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenMovieDoesNotExist()
        {
            // Arrange
            var command = new UpdateMovieCommand
            {
                MovieID = "non-existent-movie",
                MovieName = "Does not matter",
                ImageURL = new Mock<IFormFile>().Object
            };

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(command.MovieID, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Movie)null!);

            var handler = new UpdateMovieCommandHandler(
                _movieRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                new UpdateMovieValidator(),
                _publishEndpointMock.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Movie not found");

            _movieRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Movie>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<MovieUpdated>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCommandIsInvalid()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(x => x.Length).Returns(1024);

            var command = new UpdateMovieCommand
            {
                MovieID = "movie-123",
                MovieName = "",
                Genre = new List<string> { "Adventure" },
                Language = new List<string> { "French" },
                Duration = "120 mins",
                ReleaseDate = "2025-01-01",
                Rating = 4,
                AudienceScore = 4,
                CategoryID = "84f0b4ed-37eb-49e0-81e5-e643be786e04",
                SlugURL = "updated-movie",
                ImageURL = fileMock.Object
            };

            var existingMovie = new Movie
            {
                MovieID = "movie-123",
                MovieName = "Old Movie",
                ImageURL = "old-image-url"
            };

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(command.MovieID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovie);

            var handler = new UpdateMovieCommandHandler(
                _movieRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                new UpdateMovieValidator(),
                _publishEndpointMock.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");
            response.Errors.Should().NotBeNullOrEmpty();

            _movieRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Movie>()), Times.Never);
            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<MovieUpdated>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenImageIsEmpty()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(x => x.Length).Returns(0);

            var command = new UpdateMovieCommand
            {
                MovieID = "movie-123",
                MovieName = "Updated Movie",
                Genre = new List<string> { "Adventure" },
                Language = new List<string> { "French" },
                Duration = "120 mins",
                ReleaseDate = "2025-01-01",
                Rating = 4,
                AudienceScore = 4,
                CategoryID = "84f0b4ed-37eb-49e0-81e5-e643be786e04",
                SlugURL = "updated-movie",
                ImageURL = fileMock.Object
            };

            var existingMovie = new Movie
            {
                MovieID = "movie-123",
                MovieName = "Old Movie",
                ImageURL = "old-image-url"
            };

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(command.MovieID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovie);

            var handler = new UpdateMovieCommandHandler(
                _movieRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                new UpdateMovieValidator(),
                _publishEndpointMock.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Image URL is required");

            _movieRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Movie>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<MovieUpdated>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryOrFileOperationFails()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(x => x.Length).Returns(1024);

            var command = new UpdateMovieCommand
            {
                MovieID = "movie-123",
                MovieName = "Updated Movie",
                Genre = new List<string> { "Adventure" },
                Language = new List<string> { "French" },
                Duration = "120 mins",
                ReleaseDate = "2025-01-01",
                Rating = 4,
                AudienceScore = 4,
                CategoryID = "84f0b4ed-37eb-49e0-81e5-e643be786e04",
                SlugURL = "updated-movie",
                ImageURL = fileMock.Object
            };

            var existingMovie = new Movie
            {
                MovieID = "movie-123",
                MovieName = "Old Movie",
                ImageURL = "old-image-url"
            };

            _movieRepositoryMock.Setup(x => x.GetByIdAsync(command.MovieID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovie);

            _fileServiceMock.Setup(x => x.DeleteFileAsync(existingMovie.ImageURL))
                .ReturnsAsync(true);

            _fileServiceMock.Setup(x => x.UploadImageAsync(fileMock.Object))
                .ThrowsAsync(new Exception("File upload failed"));

            var handler = new UpdateMovieCommandHandler(
                _movieRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _fileServiceMock.Object,
                _loggerMock.Object,
                new UpdateMovieValidator(),
                _publishEndpointMock.Object);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");

            _movieRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Movie>()), Times.Never);
            _publishEndpointMock.Verify(x => x.Publish(It.IsAny<MovieUpdated>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}