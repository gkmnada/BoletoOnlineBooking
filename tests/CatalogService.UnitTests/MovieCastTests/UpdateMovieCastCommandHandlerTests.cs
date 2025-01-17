using AutoMapper;
using Catalog.Application.Features.MovieCast.Commands;
using Catalog.Application.Features.MovieCast.Handlers.CommandHandlers;
using Catalog.Application.Features.MovieCast.Validators;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.MovieCastTests
{
    public class UpdateMovieCastCommandHandlerTests
    {
        private readonly Mock<IMovieCastRepository> _movieCastRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UpdateMovieCastCommandHandler>> _loggerMock;
        private readonly UpdateMovieCastCommandHandler _handler;

        public UpdateMovieCastCommandHandlerTests()
        {
            _movieCastRepositoryMock = new Mock<IMovieCastRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UpdateMovieCastCommandHandler>>();

            _handler = new UpdateMovieCastCommandHandler(
                _movieCastRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                new UpdateMovieCastValidator());
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenMovieCastIsUpdated()
        {
            // Arrange
            var command = new UpdateMovieCastCommand
            {
                CastID = "cast-1",
                CastName = "Test Cast",
                Character = "Test Character",
                ImageURL = "test-image.jpg",
                MovieID = "test-movie-id",
                IsActive = true
            };

            var existingMovieCast = new MovieCast
            {
                CastID = "cast-1",
                CastName = "Old Cast",
                Character = "Old Character",
                ImageURL = "old-image.jpg",
                MovieID = "old-movie-id",
                IsActive = false
            };

            _movieCastRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieCast);

            var validator = new UpdateMovieCastValidator();
            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeTrue();

            _mapperMock.Setup(m => m.Map(command, existingMovieCast)).Returns(existingMovieCast);

            _movieCastRepositoryMock.Setup(x => x.UpdateAsync(existingMovieCast)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Movie cast updated successfully");

            _movieCastRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MovieCast>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map(command, It.IsAny<MovieCast>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenMovieCastDoesNotExist()
        {
            // Arrange
            var command = new UpdateMovieCastCommand
            {
                CastID = "non-existent-cast",
                CastName = "Does Not Matter",
                Character = "Does Not Matter",
                ImageURL = "does-not-matter.jpg",
                MovieID = "does-not-matter",
                IsActive = true
            };

            _movieCastRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((MovieCast)null!);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Movie cast not found");

            _movieCastRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _movieCastRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MovieCast>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new UpdateMovieCastCommand
            {
                CastID = "cast-1",
                CastName = "",
                Character = "",
                ImageURL = "",
                MovieID = "",
                IsActive = true
            };

            var existingMovieCast = new MovieCast
            {
                CastID = "cast-1",
                CastName = "Old Cast",
                Character = "Old Character",
                ImageURL = "old-image.jpg",
                MovieID = "old-movie-id",
                IsActive = false
            };

            _movieCastRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieCast);

            var validator = new UpdateMovieCastValidator();
            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeFalse();

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");
            response.Errors.Should().NotBeNullOrEmpty();

            _movieCastRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MovieCast>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var command = new UpdateMovieCastCommand
            {
                CastID = "cast-1",
                CastName = "Test Cast",
                Character = "Test Character",
                ImageURL = "test-image.jpg",
                MovieID = "test-movie-id",
                IsActive = true
            };

            var existingMovieCast = new MovieCast
            {
                CastID = "cast-1",
                CastName = "Old Cast",
                Character = "Old Character",
                ImageURL = "old-image.jpg",
                MovieID = "old-movie-id",
                IsActive = false
            };

            _movieCastRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieCast);

            _movieCastRepositoryMock.Setup(x => x.UpdateAsync(existingMovieCast)).Throws(new Exception());

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>();

            _movieCastRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MovieCast>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}