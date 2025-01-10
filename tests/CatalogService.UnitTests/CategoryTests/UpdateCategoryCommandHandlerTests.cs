using AutoMapper;
using Catalog.Application.Features.Category.Commands;
using Catalog.Application.Features.Category.Handlers.CommandHandlers;
using Catalog.Application.Features.Category.Validators;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CatalogService.UnitTests.CategoryTests
{
    public class UpdateCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UpdateCategoryCommandHandler>> _loggerMock;
        private readonly UpdateCategoryCommandHandler _handler;

        public UpdateCategoryCommandHandlerTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UpdateCategoryCommandHandler>>();

            _handler = new UpdateCategoryCommandHandler(
                _categoryRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                new UpdateCategoryValidator());
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCategoryIsUpdated()
        {
            // Arrange
            var command = new UpdateCategoryCommand
            {
                CategoryID = "category-1",
                Name = "Test Category",
                SlugURL = "test-category"
            };

            var existingCategory = new Category
            {
                CategoryID = "category-1",
                Name = "Old Category",
                SlugURL = "old-category"
            };

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            var validator = new UpdateCategoryValidator();
            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeTrue();

            _mapperMock.Setup(m => m.Map(command, existingCategory)).Returns(existingCategory);

            _categoryRepositoryMock.Setup(x => x.UpdateAsync(existingCategory)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Category updated successfully");
            response.Data.Should().NotBeNull();

            _categoryRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Category>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map(command, It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var command = new UpdateCategoryCommand
            {
                CategoryID = "non-existent-movie",
                Name = "Does Not Matter",
                SlugURL = "does-not-matter"
            };

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category)null!);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Category not found");

            _categoryRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _categoryRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new UpdateCategoryCommand
            {
                CategoryID = "movie-1",
                Name = "",
                SlugURL = ""
            };

            var existingCategory = new Category
            {
                CategoryID = "movie-1",
                Name = "Old Category",
                SlugURL = "old-category"
            };

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            var validator = new UpdateCategoryValidator();
            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.Should().BeFalse();

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");
            response.Errors.Should().NotBeNullOrEmpty();

            _categoryRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Category>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            var command = new UpdateCategoryCommand
            {
                CategoryID = "category-1",
                Name = "Test Category",
                SlugURL = "test-category"
            };

            var existingCategory = new Category
            {
                CategoryID = "category-1",
                Name = "Old Category",
                SlugURL = "old-category"
            };

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepositoryMock.Setup(x => x.UpdateAsync(existingCategory)).Throws(new Exception());

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>();

            _categoryRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Category>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
