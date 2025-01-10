using Catalog.Application.Features.Category.Commands;
using Catalog.Application.Features.Category.Handlers.CommandHandlers;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.CategoryTests
{
    public class DeleteCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<DeleteCategoryCommandHandler>> _loggerMock;
        private readonly DeleteCategoryCommandHandler _handler;

        public DeleteCategoryCommandHandlerTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DeleteCategoryCommandHandler>>();

            _handler = new DeleteCategoryCommandHandler(
                _categoryRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCategoryIsDeleted()
        {
            // Arrange
            var command = new DeleteCategoryCommand("category-1");

            var existingCategory = new Category
            {
                CategoryID = "category-1",
                Name = "Test Category",
                SlugURL = "test-category"
            };

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(command.CategoryID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepositoryMock.Setup(x => x.DeleteAsync(existingCategory)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Category deleted successfully");

            _categoryRepositoryMock.Verify(x => x.GetByIdAsync(command.CategoryID, It.IsAny<CancellationToken>()), Times.Once);
            _categoryRepositoryMock.Verify(x => x.DeleteAsync(existingCategory), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var command = new DeleteCategoryCommand("category-1");

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(command.CategoryID, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category)null!);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Category not found");

            _categoryRepositoryMock.Verify(x => x.GetByIdAsync(command.CategoryID, It.IsAny<CancellationToken>()), Times.Once);
            _categoryRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var command = new DeleteCategoryCommand("category-1");

            var existingCategory = new Category
            {
                CategoryID = "category-1",
                Name = "Test Category",
                SlugURL = "test-category"
            };

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(command.CategoryID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepositoryMock.Setup(x => x.DeleteAsync(existingCategory)).Throws(new Exception("An error occurred while deleting the category"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("An error occurred while processing your request");

            _categoryRepositoryMock.Verify(x => x.GetByIdAsync(command.CategoryID, It.IsAny<CancellationToken>()), Times.Once);
            _categoryRepositoryMock.Verify(x => x.DeleteAsync(existingCategory), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
