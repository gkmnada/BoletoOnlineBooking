using AutoMapper;
using Catalog.Application.Features.MovieCast.Commands;
using Catalog.Application.Features.MovieCast.Handlers.CommandHandlers;
using Catalog.Application.Features.MovieCast.Validators;
using Catalog.Application.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.MovieCastTests
{
    public class CreateMovieCastCommandHandlerTests
    {
        private readonly Mock<IMovieCastRepository> _movieCastRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateMovieCastCommandHandler>> _loggerMock;
        private readonly CreateMovieCastCommandHandler _handler;

        public CreateMovieCastCommandHandlerTests()
        {
            _movieCastRepositoryMock = new Mock<IMovieCastRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateMovieCastCommandHandler>>();

            _handler = new CreateMovieCastCommandHandler(
                _movieCastRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                new CreateMovieCastValidator());
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenMovieCastsAreCreated()
        {
            // Arrange
            var command = new CreateMovieCastCommand
            {
                MovieCasts = new List<CreateMovieCastItem>
                {
                    new CreateMovieCastItem
                    {
                        CastName = "Test Cast",
                        Character = "Test Character",
                        ImageURL = "test-image.jpg",
                        MovieID = "test-movie-id"
                    }
                }
            };

            var validator = new CreateMovieCastValidator();
            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeTrue();

            _movieCastRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Catalog.Domain.Entities.MovieCast>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Movie casts created successfully");

            _movieCastRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Catalog.Domain.Entities.MovieCast>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreateMovieCastCommand
            {
                MovieCasts = new List<CreateMovieCastItem>
                {
                    new CreateMovieCastItem
                    {
                        CastName = "",
                        Character = "",
                        ImageURL = "",
                        MovieID = ""
                    }
                }
            };

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");
            response.Errors.Should().NotBeEmpty();

            _movieCastRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Catalog.Domain.Entities.MovieCast>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var command = new CreateMovieCastCommand
            {
                MovieCasts = new List<CreateMovieCastItem>
                {
                    new CreateMovieCastItem
                    {
                        CastName = "Test Cast",
                        Character = "Test Character",
                        ImageURL = "test-image.jpg",
                        MovieID = "test-movie-id"
                    }
                }
            };

            _movieCastRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Catalog.Domain.Entities.MovieCast>())).Throws(new Exception());

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>();

            _movieCastRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Catalog.Domain.Entities.MovieCast>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}