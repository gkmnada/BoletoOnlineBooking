using Catalog.Application.Features.MovieCast.Commands;
using Catalog.Application.Features.MovieCast.Handlers.CommandHandlers;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.MovieCastTests
{
    public class DeleteMovieCastCommandHandlerTests
    {
        private readonly Mock<IMovieCastRepository> _movieCastRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<DeleteMovieCastCommandHandler>> _loggerMock;
        private readonly DeleteMovieCastCommandHandler _handler;

        public DeleteMovieCastCommandHandlerTests()
        {
            _movieCastRepositoryMock = new Mock<IMovieCastRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DeleteMovieCastCommandHandler>>();

            _handler = new DeleteMovieCastCommandHandler(
                _movieCastRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenMovieCastIsDeleted()
        {
            // Arrange
            var command = new DeleteMovieCastCommand("cast-1");

            var existingMovieCast = new MovieCast
            {
                CastID = "cast-1",
                CastName = "Test Cast",
                Character = "Test Character",
                ImageURL = "test-image.jpg",
                MovieID = "test-movie-id"
            };

            _movieCastRepositoryMock.Setup(x => x.GetByIdAsync(command.CastID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieCast);

            _movieCastRepositoryMock.Setup(x => x.DeleteAsync(existingMovieCast)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Movie cast deleted successfully");
            response.Data.Should().Be(existingMovieCast.MovieID);

            _movieCastRepositoryMock.Verify(x => x.GetByIdAsync(command.CastID, It.IsAny<CancellationToken>()), Times.Once);
            _movieCastRepositoryMock.Verify(x => x.DeleteAsync(existingMovieCast), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenMovieCastDoesNotExist()
        {
            // Arrange
            var command = new DeleteMovieCastCommand("cast-1");

            _movieCastRepositoryMock.Setup(x => x.GetByIdAsync(command.CastID, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MovieCast)null!);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Movie cast not found");

            _movieCastRepositoryMock.Verify(x => x.GetByIdAsync(command.CastID, It.IsAny<CancellationToken>()), Times.Once);
            _movieCastRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<MovieCast>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var command = new DeleteMovieCastCommand("cast-1");

            var existingMovieCast = new MovieCast
            {
                CastID = "cast-1",
                CastName = "Test Cast",
                Character = "Test Character",
                ImageURL = "test-image.jpg",
                MovieID = "test-movie-id"
            };

            _movieCastRepositoryMock.Setup(x => x.GetByIdAsync(command.CastID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingMovieCast);

            _movieCastRepositoryMock.Setup(x => x.DeleteAsync(existingMovieCast))
                .Throws(new Exception("An error occurred while deleting the movie cast"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing the request");

            _movieCastRepositoryMock.Verify(x => x.GetByIdAsync(command.CastID, It.IsAny<CancellationToken>()), Times.Once);
            _movieCastRepositoryMock.Verify(x => x.DeleteAsync(existingMovieCast), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}